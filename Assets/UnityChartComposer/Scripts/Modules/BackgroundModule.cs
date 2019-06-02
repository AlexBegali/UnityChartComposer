using UnityEngine;

namespace ChartModuleSystem.Modules
{
    [CreateAssetMenu(fileName = "BackgroundModule", menuName = "Chart Module System/Background Module", order = 1)]
    public class BackgroundModule : ModuleBase, IModule
    {
        [Space]
        public Color BackgroundColor;
        public string ColorField;

        private Color ActualColor;

        void IModule.Initialize()
        {
            KernelIndex = ModuleShader.FindKernel(KernelName);
            ActualColor = BackgroundColor;

            ModuleShader.SetVector(ColorField, ActualColor);
        }

        protected override void CreateComputeBuffer()
        {

        }

        void IModule.Dispatch()
        {
            if (ActualColor != BackgroundColor)
            {
                ActualColor = BackgroundColor;
                ModuleShader.SetVector(ColorField, ActualColor);
            }

            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {

        }
    }
}