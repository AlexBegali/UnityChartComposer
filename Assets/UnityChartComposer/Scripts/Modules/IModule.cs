using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChartModuleSystem
{
    public interface IModule
    {
        void Initialize();
        void SetRenderTexture(RenderTexture rt);
        void Dispatch();
        void Dispose();
    }
}