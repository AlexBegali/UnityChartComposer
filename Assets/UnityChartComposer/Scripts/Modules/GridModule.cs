using UnityEngine;

namespace ChartModuleSystem.Modules
{
    [CreateAssetMenu(fileName = "GridModule", menuName = "Chart Module System/Grid Module", order = 2)]
    public class GridModule : ModuleBase, IModule
    {
        [Space]
        public Color GridColor;
        public string ColorField;

        [Space]
        public Vector2 GridAnchor;
        public string GridAnchorField;

        [Space]
        public float GridScale;
        public string GridScaleField;

        [Space]
        public float GridThickness;
        public string GridThicknessField;

        private Color ActualColor;
        private Vector2 ActualAnchor;
        private float ActualScale;
        private float ActualThickness;

        void IModule.Initialize()
        {
            KernelIndex = ModuleShader.FindKernel(KernelName);

            ActualColor = GridColor;
            ActualAnchor = GridAnchor;
            ActualScale = GridScale;

            ModuleShader.SetVector(ColorField, ActualColor);
            ModuleShader.SetFloats(GridAnchorField, ActualAnchor.x, ActualAnchor.y);
            ModuleShader.SetFloat(GridScaleField, ActualScale);
            ModuleShader.SetFloat(GridThicknessField, GridThickness);
        }

        protected override void CreateComputeBuffer()
        {

        }

        void IModule.Dispatch()
        {
            if (ActualColor != GridColor)
            {
                ActualColor = GridColor;
                ModuleShader.SetVector(ColorField, ActualColor);
            }

            if (ActualAnchor != GridAnchor)
            {
                ActualAnchor = GridAnchor;
                ModuleShader.SetFloats(GridAnchorField, ActualAnchor.x, ActualAnchor.y);
            }

            if (ActualScale != GridScale)
            {
                if (GridScale <= 1) GridScale = 1;

                ActualScale = GridScale;
                ModuleShader.SetFloat(GridScaleField, ActualScale);
            }

            if (ActualThickness != GridThickness)
            {
                if (GridThickness < 0) GridThickness = 0;

                ActualThickness = GridThickness;
                ModuleShader.SetFloat(GridThicknessField, GridThickness);
            }

            ModuleShader.Dispatch(KernelIndex, Resolution.x, Resolution.y, 1);
        }

        void IModule.Dispose()
        {

        }
    }
}