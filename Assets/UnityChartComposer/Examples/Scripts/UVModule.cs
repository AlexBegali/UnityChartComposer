using ChartModuleSystem.Modules;

namespace ChartModuleSystem.Examples
{
    //[CreateAssetMenu(fileName = "UVModule", menuName = "Chart Module System/UV Module", order = 0)]
    public class UVModule : ModuleBase, IModule
    {
        void IModule.Initialize()
        {
            KernelIndex = ModuleShader.FindKernel(KernelName);
        }

        protected override void CreateComputeBuffer()
        {

        }

        void IModule.Dispatch()
        {
            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {

        }
    }
}