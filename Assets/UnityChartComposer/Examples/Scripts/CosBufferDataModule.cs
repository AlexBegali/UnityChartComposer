using ChartModuleSystem.Modules;
using UnityEngine;

namespace ChartModuleSystem.Examples
{
    //[CreateAssetMenu(fileName = "BufferDataModule", menuName = "Chart Module System/Buffer Data Module", order = 4)]
    public class CosBufferDataModule : ModuleBase, IModule
    {
        [Space]
        public Color ChartColor;
        public string ColorField;

        [Space]
        public string ComputeBufferField = "dataBuffer";

        [Space]
        public float LineThickness;
        public string LineThicknessField;

        [Space]
        public float Scale_X = 50.0f;
        public float Scale_Y = 2.0f;
        public float Speed = 100.0f;

        private Color ActualColor;
        private float ActualThickness;

        void IModule.Initialize()
        {
            KernelIndex = ModuleShader.FindKernel(KernelName);
            ActualColor = ChartColor;

            ModuleShader.SetVector(ColorField, ActualColor);
            ModuleShader.SetFloat(LineThicknessField, LineThickness);
        }

        protected override void CreateComputeBuffer()
        {
            ComputeBuffer cb = ChartComputeBuffer;

            ChartComputeBuffer = new ComputeBuffer(Resolution.x, sizeof(float));
            DataArray = new float[Resolution.x];

            ModuleShader.SetBuffer(KernelIndex, ComputeBufferField, ChartComputeBuffer);

            if (cb != null)
            {
                cb.Release();
            }
        }

        void IModule.Dispatch()
        {
            if (ActualColor != ChartColor)
            {
                ActualColor = ChartColor;
                ModuleShader.SetVector(ColorField, ActualColor);
            }

            if (ActualThickness != LineThickness)
            {
                if (LineThickness < 0) LineThickness = 0;

                ActualThickness = LineThickness;
                ModuleShader.SetFloat(LineThicknessField, LineThickness);
            }

            for (int i = 0; i < DataArray.Length; i++)
            {
                DataArray[i] = (Mathf.Cos((i + Time.realtimeSinceStartup * Speed) / Scale_X) + Scale_Y) / (Scale_Y * 2.0f);
            }

            ChartComputeBuffer.SetData(DataArray);

            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {
            if (ChartComputeBuffer != null) ChartComputeBuffer.Release();
        }
    }
}