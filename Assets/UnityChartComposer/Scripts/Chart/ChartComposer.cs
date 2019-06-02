using ChartModuleSystem.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChartModuleSystem
{
    public class ChartComposer : MonoBehaviour
    {
        public string ChartName;
        public GameObject ChartGameObject;

        [Space]
        public bool AutoSize = false;
        public Rect ChartMaxSize = new Rect(1, 1, 2048, 2048);

        [Space]
        public int RenderTextureDepth = 32;

        [Space]
        public List<ModuleBase> Modules;
        
        private RawImage ChartImage;

        private Vector2 ChartSize;
        private Vector2Int CeiledChartSize;

        private RenderTexture ChartRenderTexture;

        private void Awake()
        {
            CleanUpModules(Modules);

            ChartGameObject = GetChartGameObject(ChartGameObject);
            ChartName = GetChartName(ChartName, ChartGameObject);
            ChartImage = GetChartImage(ChartGameObject);
            ChartSize = GetChartSize(ChartImage);
            CeiledChartSize = CeilVector2(ChartSize);

            ChartRenderTexture = SetRenderTexture(ChartMaxSize, ChartImage, RenderTextureDepth, CeiledChartSize, ChartRenderTexture);

            foreach (IModule module in Modules)
            {
                InitializeModule(module);
                SetModuleRenderTexture(module, ChartRenderTexture);
            }
        }

        private void CleanUpModules(List<ModuleBase> modules)
        {
            for (int i = modules.Count - 1; i >= 0; i--)
            {
                if (modules[i] == null) modules.RemoveAt(i);
            }
        }

        private GameObject GetChartGameObject(GameObject holder)
        {
            return holder != null ? holder : gameObject;
        }

        private string GetChartName(string currentName, GameObject holder)
        {
            return string.IsNullOrEmpty(currentName) ? currentName : holder.name;
        }

        private RawImage GetChartImage(GameObject holder)
        {
            RawImage graphic = holder.GetComponent<RawImage>();
            if (graphic == null) throw new NullReferenceException("There is or RawImage component on " + ChartGameObject + " GameObject");

            return graphic;
        }

        private Vector2 GetChartSize(RawImage graphic)
        {
            return graphic.rectTransform.rect.size;
        }

        private Vector2Int CeilVector2(Vector2 chartSize)
        {
            return Vector2Int.CeilToInt(chartSize);
        }

        private RenderTexture SetRenderTexture(Rect maxSize, RawImage graphic, int depth, Vector2Int ceiledChartSize, RenderTexture texture)
        {
            if (maxSize.Contains(graphic.rectTransform.rect.size))
            {
                RenderTexture old = texture;

                texture = new RenderTexture(ceiledChartSize.x, ceiledChartSize.y, depth)
                {
                    enableRandomWrite = true,
                    name = "renderTexture-" + ceiledChartSize.x + "-" + ceiledChartSize.y,
                };

                texture.Create();
                texture.filterMode = FilterMode.Point;

                graphic.texture = texture;

                if (old != null)
                {
                    old.Release();
                }
            }

            return texture;
        }

        private void InitializeModule(IModule module)
        {
            module.Initialize();
        }

        private void SetModuleRenderTexture(IModule module, RenderTexture rTexture)
        {
            module.SetRenderTexture(rTexture);
        }
        
        private void Update()
        {
            if (AutoSize && CheckSizeActuality(ChartSize, ChartImage) == false)
            {
                ChartSize = GetChartSize(ChartImage);
                CeiledChartSize = CeilVector2(ChartSize);
                
                ChartRenderTexture = SetRenderTexture(ChartMaxSize, ChartImage, RenderTextureDepth, CeiledChartSize, ChartRenderTexture);

                foreach (IModule module in Modules)
                {
                    SetModuleRenderTexture(module, ChartRenderTexture);
                }
            }

            foreach (IModule module in Modules)
            {
                module.Dispatch();
            }
        }

        private bool CheckSizeActuality(Vector2 chartSize, RawImage graphic)
        {
            return chartSize == graphic.rectTransform.rect.size;
        }

        private void OnDestroy()
        {
            foreach (IModule module in Modules)
            {
                module.Dispose();
            }

            if (ChartRenderTexture != null) ChartRenderTexture.Release();
        }
    }    
}