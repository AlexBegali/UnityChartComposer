using ChartModuleSystem.Modules;
using UnityEngine;

namespace ChartModuleSystem.Examples
{
    //[CreateAssetMenu(fileName = "CosSimpleModule", menuName = "Chart Module System/Cos Simple Module", order = 3)]
    public class CosSimpleModule : ModuleBase, IModule
    {
        [Space]
        public Color ChartColor;
        public string ColorField;

        private Color ActualColor;

        void IModule.Initialize()
        {
            KernelIndex = ModuleShader.FindKernel(KernelName);
            ActualColor = ChartColor;

            ModuleShader.SetVector(ColorField, ActualColor);
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

            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {
           
        }
    }
}