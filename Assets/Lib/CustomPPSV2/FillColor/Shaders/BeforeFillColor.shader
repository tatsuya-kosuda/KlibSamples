// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/Custom/BeforeFillColor"
{
    Properties
    {
        _TopColor ("Color", Color) = (0,0,0,0)
        _TopColorPos("Top Color Position", float) = 1.0
        _BottomColorPos("Bottom Color Position", float) = 0
        _Offset("Gradation Offset", float) = 0
        _FillAmount("Fill Amount", Range(0, 1)) = 1
        _BottomColor("Bottom Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float3 wpos : TEXCOORD1;
            float3 lpos : TEXCOORD2;
        };

        fixed4 _TopColor;
        fixed _TopColorPos;
        fixed _BottomColorPos;
        fixed _Offset;
        fixed _FillAmount;
        fixed4 _BottomColor;
        fixed _Alpha;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.wpos = mul (unity_ObjectToWorld, v.vertex).xyz;
            o.lpos = o.wpos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
            o.uv = v.uv;
            return o;
        }

        fixed4 fragDefault (v2f i) : SV_Target
        {
            fixed amount = clamp((i.wpos.y - _BottomColorPos) / (_TopColorPos - _BottomColorPos) - _Offset, 0, 1);
            return lerp(_BottomColor, _TopColor, amount) * _FillAmount;
        }

        fixed4 fragScreen (v2f i) : SV_Target
        {
            fixed amount = clamp((i.wpos.y - _BottomColorPos) / (_TopColorPos - _BottomColorPos) - _Offset, 0, 1);
            return lerp(_BottomColor, _TopColor, amount) * _FillAmount;
        }

        fixed4 fragOverlay (v2f i) : SV_Target
        {
            fixed amount = clamp((i.wpos.y - _BottomColorPos) / (_TopColorPos - _BottomColorPos) - _Offset, 0, 1);
            fixed4 res = lerp(_BottomColor, _TopColor, amount);
            res.a = _FillAmount;
            return res;
        }
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragDefault
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragScreen
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOverlay
            ENDCG
        }
        Pass
        {
            ZWrite On
            ColorMask 0
        }
    }
}
