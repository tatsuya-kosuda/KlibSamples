using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace Kosu.UnityLibrary
{
    public class FillColorRenderer : PostProcessEffectRenderer<FillColorSettings>
    {

        private FillColorTarget[] _targets;

        private static readonly Shader FILLCOLOR_SHADER  = Shader.Find("Hidden/Custom/FillColor");

        private static readonly Shader BEFORE_FILLCOLOR_SHADER = Shader.Find("Hidden/Custom/BeforeFillColor");

        private static readonly int FILLCOLOR = Shader.PropertyToID("_FillColor");

        private static readonly int TOP_COLOR = Shader.PropertyToID("_TopColor");

        private static readonly int TOP_COLOR_POS = Shader.PropertyToID("_TopColorPos");

        private static readonly int BOTTOM_COLOR = Shader.PropertyToID("_BottomColor");

        private static readonly int BOTTOM_COLOR_POS = Shader.PropertyToID("_BottomColorPos");

        private static readonly int OFFSET = Shader.PropertyToID("_Offset");

        private static readonly int FILL_AMOUNT = Shader.PropertyToID("_FillAmount");

        private static readonly int OVERLAY_THREASHOLD = Shader.PropertyToID("_OverlayThreashold");

        private static readonly int OVERLAY_INTENSITY = Shader.PropertyToID("_OverlayIntensity");

        private static readonly int OVERLAY_GAIN = Shader.PropertyToID("_OverlayGain");

        public override void Init()
        {
            base.Init();
            SetupFillcolorTargets();
        }

        public override void Render(PostProcessRenderContext context)
        {
            if (Application.isPlaying == false)
            {
                SetupFillcolorTargets();
            }

            var cmd = context.command;
            cmd.BeginSample("FillColor");
            var sheet = context.propertySheets.Get(FILLCOLOR_SHADER);
            // cmd.setrendertargetはうまく動かなかった... forwardだと動かない??
            cmd.GetTemporaryRT(FILLCOLOR, -1, -1, 24);
            cmd.BlitFullscreenTriangle(BuiltinRenderTextureType.CurrentActive, FILLCOLOR);
            // NOTE : stencil使えばべつにclearしなくてもいいかな
            cmd.ClearRenderTarget(true, true, Color.black);

            if (_targets != null)
            {
                foreach (var target in _targets)
                {
                    Material mat;

                    if (target.IsEraseColor)
                    {
                        mat = new Material(BEFORE_FILLCOLOR_SHADER);
                        mat.SetColor(TOP_COLOR, Color.black);
                        mat.SetColor(BOTTOM_COLOR, Color.black);
                    }
                    else
                    {
                        mat = new Material(BEFORE_FILLCOLOR_SHADER);
                        int fillColorIndex = Mathf.Clamp(target.ColorIndex, 0, settings.fillColor.value.Length);
                        mat.SetColor(TOP_COLOR, settings.fillColor.value[fillColorIndex]);
                        int fillBottomColorIndex = Mathf.Clamp(target.BottomColorIndex, 0, settings.bottomFillColor.value.Length);
                        mat.SetColor(BOTTOM_COLOR, settings.bottomFillColor.value[fillBottomColorIndex]);
                        mat.SetFloat(TOP_COLOR_POS, target.TopColorPos);
                        mat.SetFloat(BOTTOM_COLOR_POS, target.BottomColorPos);
                        mat.SetFloat(OFFSET, target.Offset);
                        mat.SetFloat(FILL_AMOUNT, target.FillAmount);
                    }

                    for (int i = 0; i < target.Renderer.Length; i++)
                    {
                        var renderer = target.Renderer[i];
                        var meshFilter = target.MeshFilter[i];

                        for (int j = 0; j < meshFilter.sharedMesh.subMeshCount; j++)
                        {
                            cmd.DrawRenderer(renderer, mat, j, (int) settings.mode.value);
                        }
                    }
                }
            }

            sheet.properties.SetFloat(OVERLAY_THREASHOLD, settings.overlayThreashold);
            sheet.properties.SetFloat(OVERLAY_INTENSITY, settings.overlayIntensity);
            sheet.properties.SetFloat(OVERLAY_GAIN, settings.overlayGain);
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int) settings.mode.value);
            cmd.ReleaseTemporaryRT(FILLCOLOR);
            cmd.EndSample("FillColor");
        }

        private void SetupFillcolorTargets()
        {
            _targets = GameObject.FindObjectsOfType<FillColorTarget>();
        }
    }
}
