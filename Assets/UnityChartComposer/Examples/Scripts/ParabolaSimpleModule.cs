using ChartModuleSystem.Modules;
using UnityEngine;

namespace ChartModuleSystem.Examples
{
    //[CreateAssetMenu(fileName = "ParabolaSimpleModule", menuName = "Chart Module System/Parabola Simple Module", order = 5)]
    public class ParabolaSimpleModule : ModuleBase, IModule
    {
        [Space]
        public Color ChartColor;
        public string ColorField;

        [Space]
        public float LineThickness;
        public string LineThicknessField;

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

            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {
            if (ChartComputeBuffer != null) ChartComputeBuffer.Release();
        }
    }
}