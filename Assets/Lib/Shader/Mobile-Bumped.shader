// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "MyShader/Mobile/Bumped Diffuse" {
Properties {
	_Color("Color", Color) = (1, 1, 1, 1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 250

CGPROGRAM
#pragma surface surf Lambert addshadow
#pragma multi_compile_instancing

sampler2D _MainTex;
sampler2D _BumpMap;

struct Input {
    float2 uv_MainTex;
};

UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
#define _Color_arr Props
UNITY_INSTANCING_BUFFER_END(Props)

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
    o.Albedo = c.rgb;
    o.Alpha = c.a;
    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG
}

FallBack "Mobile/Diffuse"
}
