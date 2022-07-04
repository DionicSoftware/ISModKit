Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags { "Queue" = "Geometry+9" }

        ColorMask 0
        ZWrite On

        Pass {}
    }
}
