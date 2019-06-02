using System;
using UnityEngine;

namespace ChartModuleSystem.Modules
{
    public abstract class ModuleBase : ScriptableObject
    {
        public ComputeShader ModuleShader;
        public string KernelName = "main";

        [Space]
        public string RenderTextureField = "output";
        public string ResolutionField = "resolution";
        
        protected int KernelIndex;
        protected Vector2Int Resolution;

        protected ComputeBuffer ChartComputeBuffer;
        protected float[] DataArray;

        public void SetRenderTexture(RenderTexture rt)
        {
            ModuleShader.SetTexture(KernelIndex, RenderTextureField, rt);
            Resolution = new Vector2Int(rt.width, rt.height);
            ModuleShader.SetFloats(ResolutionField, Resolution.x, Resolution.y);

            CreateComputeBuffer();
        }

        protected abstract void CreateComputeBuffer();
    }
}