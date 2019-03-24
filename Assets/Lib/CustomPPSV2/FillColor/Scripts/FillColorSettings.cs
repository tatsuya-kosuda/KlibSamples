using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kosu.UnityLibrary
{
    [System.Serializable]
    [PostProcess(typeof(FillColorRenderer), PostProcessEvent.BeforeStack, "Custom/FillColor")]
    public sealed class FillColorSettings : PostProcessEffectSettings
    {

        public FillColorModeParameter mode = new FillColorModeParameter();

        public ColorArrayParameter fillColor = new ColorArrayParameter();

        public ColorArrayParameter bottomFillColor = new ColorArrayParameter();

        [Range(0, 1)]
        public FloatParameter overlayThreashold = new FloatParameter();

        [Range(1, 4)]
        public FloatParameter overlayIntensity = new FloatParameter();

        [Range(0, 1)]
        public FloatParameter overlayGain = new FloatParameter();

    }
}
