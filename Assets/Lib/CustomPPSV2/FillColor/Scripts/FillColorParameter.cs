using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kosu.UnityLibrary
{
    public enum FillColorMode
    {
        ADD,
        SCREEN,
        OVERLAY
    }

    [System.Serializable]
    public sealed class ColorArrayParameter : ParameterOverride<Color[]>
    {
    }

    [System.Serializable]
    public sealed class FillColorModeParameter : ParameterOverride<FillColorMode>
    {
    }

}
