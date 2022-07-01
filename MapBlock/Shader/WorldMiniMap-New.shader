Shader "SLG_Custom/Scene/World/WorldMiniMap-New"
{
    Properties
    {
        [MainTexture]_DataMap_Color ("DataTex Data", 2D) = "black" {}
        _DataMap_Color2 ("DataTex Data2", 2D) = "black" {}
        _DataMap_Edge ("DataTex Edge", 2D) = "black" {}
        _DataMap_Angle ("DataTex Angle", 2D) = "black" {}
        _ShapeMap_Edge ("ShapeTex Edge", 2D) = "black" {}
        _ShapeMap_Angle ("ShapeTex Angle", 2D) = "black" {}

        _ShapeColorStrength ("Shape Color Strength", float) = 1

        _OffsetFactor("OffsetFactor",Float) = -2
        _OffsetUnits("OffsetUnits",Float) = 0

        _IsHide("IsHide",Float) = 0

        //-------------------------------------------------
        //               Obsolete Properties
        //-------------------------------------------------
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
    }


    SubShader
    {
        Tags 
        {  
            "Queue"="Transparent+1" 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "IgnoreProjector" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        Offset [_OffsetFactor],[_OffsetUnits]

        Pass
        {
            Name "ForwardLit"

            HLSLPROGRAM
            
            // #pragma enable_d3d11_debug_symbols
            
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma target 3.0
            
            #include "WorldMiniMap-New-Input.hlsl"
            #include "WorldMiniMap-New-Pass.hlsl"

            ENDHLSL
        }
    }

    FallBack "SLG_Custom/CommonShader/MaterialError"
}
