#ifndef WORLD_MINIMAP_NEW_PASS_INCLUDED
    #define WORLD_MINIMAP_NEW_PASS_INCLUDED
    

    struct Attributes
    {
        float4 vertex    : POSITION;
        float2 texcoord      : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 vertex   : SV_POSITION;
        float2 texcoord  : TEXCOORD0;
        
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };


    // Used in Standard (Simple Lighting) shader
    Varyings Vertex(Attributes input)
    {
        Varyings output = (Varyings)0;

        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_TRANSFER_INSTANCE_ID(input, output);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);

        output.texcoord = input.texcoord;
        output.vertex = vertexInput.positionCS;

        return output;
    }

    // Used for StandardSimpleLighting shader
    half4 Fragment(Varyings input) : SV_Target
    {
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        if(_IsHide)
        return half4(0,0,0,0);

        half2 dataUv = TRANSFORM_TEX(input.texcoord,_DataMap_Color);
        half4 data_edge = SAMPLE_TEXTURE2D(_DataMap_Edge, sampler_DataMap_Edge, dataUv);
        half4 data_angle = SAMPLE_TEXTURE2D(_DataMap_Angle, sampler_DataMap_Angle, dataUv);
        half4 data_color = SAMPLE_TEXTURE2D(_DataMap_Color, sampler_DataMap_Color, dataUv);
        half4 data_color2 = SAMPLE_TEXTURE2D(_DataMap_Color2, sampler_DataMap_Color2, dataUv);

        uint index = dot(data_edge,uint4(1,2,4,8));
        uint x = index % 4;
        uint y = floor(index* 0.25);

        uint index2 = dot(data_angle,uint4(1,2,4,8));
        uint x2 = index2 % 4;
        uint y2 = floor(index2* 0.25);
        
        half2 uv = frac(input.texcoord * 17);
        half2 uv_1 = (uv + half2(x,y)) * 0.25;
        half2 uv_2 = (uv + half2(x2,y2)) * 0.25;
        
        half shape_edge = SAMPLE_TEXTURE2D(_ShapeMap_Edge, sampler_ShapeMap_Edge, uv_1).r ;
        half shape_angle = SAMPLE_TEXTURE2D(_ShapeMap_Angle, sampler_ShapeMap_Angle, uv_2).r;

        half4 col = lerp(data_color ,data_color2 ,(shape_edge + shape_angle)*_ShapeColorStrength) * (1-step(data_color.a,0));
        return col;

    }

#endif
