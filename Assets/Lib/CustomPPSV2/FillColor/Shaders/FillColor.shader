Shader "Hidden/Custom/FillColor"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_FillColor, sampler_FillColor);
    float _OverlayThreashold;
    float _OverlayIntensity;
    float _OverlayGain;

    float4 fragDefault (VaryingsDefault i) : SV_Target
    {
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float4 fillCol = SAMPLE_TEXTURE2D(_FillColor, sampler_FillColor, i.texcoord);
        return col + fillCol;
    }

    float4 fragScreen (VaryingsDefault i) : SV_Target
    {
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float4 fillCol = SAMPLE_TEXTURE2D(_FillColor, sampler_FillColor, i.texcoord);
        return col + fillCol - (col * fillCol);
    }

    float4 fragOverlay (VaryingsDefault i) : SV_Target
    {
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float4 fillCol = SAMPLE_TEXTURE2D(_FillColor, sampler_FillColor, i.texcoord);

        if (fillCol.r < 0.001 && fillCol.g < 0.001 && fillCol.b < 0.001)
        {
            // 黒は原色で描画
            return col;
        }

        if (col.r < _OverlayThreashold && col.g < _OverlayThreashold && col.b < _OverlayThreashold)
        {
            float4 res = lerp(col, _OverlayIntensity * col * fillCol, _OverlayGain * fillCol.a);
            //res.a = fillCol.a;
            return res;
        }
        else
        {
            float4 res = lerp(col, 1 - _OverlayIntensity * (1 - col) * (1 - fillCol), _OverlayGain * fillCol.a);
            //res.a = fillCol.a;
            return res;
        }
    }

    ENDHLSL

    SubShader
    {

        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment fragDefault
            ENDHLSL
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment fragScreen
            ENDHLSL
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment fragOverlay
            ENDHLSL
        }

    }
}
