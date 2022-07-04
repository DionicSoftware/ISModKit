Shader "Custom/Masked"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB) / EmissionMask(A)", 2D) = "white" {}
        _MaskTex("Metallic/Mask/AO/Smoothness (RGBA)", 2D) = "white" {}
        _MaskColor("Mask Color", Color) = (1,1,1,1)
        _NormalTex("Normal", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _AO("AO", Range(0,1)) = 0.0
        _Tint("Tint", Color) = (0,0,0,0)
        [HDR] _EmissiveColor("Emissive Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Stencil{
            Ref 2 // 1 is for terrain, 2 is for building, 3 for road
            Comp Always
            Pass Replace
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MaskTex;
        sampler2D _NormalTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
            float2 uv_NormalTex;
        };

        half _Glossiness;
        half _Metallic;
        half _AO;
        fixed4 _Color;
        fixed4 _MaskColor;
        fixed4 _Tint;
        fixed4 _EmissiveColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 mask = tex2D(_MaskTex, IN.uv_MaskTex);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * lerp(_MaskColor, _Color, mask.g);
            o.Albedo = c.rgb * lerp(_AO, 1, mask.b);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic * mask.r;
            o.Smoothness = _Glossiness * mask.a;
            o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
            o.Alpha = c.a;
            o.Emission = _Tint + c.a * _EmissiveColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
