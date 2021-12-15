Shader "BushShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        Vector1_4a9c697724064c6dbc765e7bc85f89c7("WindSpeed", Float) = 0.4
        Vector2_49f7f77a33904423ad6b5bbc47d673aa("WindDir", Vector) = (1, 0, 0, 0)
        Vector1_dec963672df24678a19b17b8fef25c38("WindScale", Float) = 1
        Vector1_2b12f4302b4e4c779cce0997b283c146("WindStrength", Float) = 0.01
        Vector1_e1ec7c947fa74423b9e01b6f1234c57b("MaskPosY", Float) = 1
        Vector1_1a79ced4a0574d24b95f79288f00ec55("GrassSaturation", Float) = 1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        _Stencil("Stencil ID", Float) = 0
    _StencilComp("StencilComp", Float) = 8
    _StencilOp("StencilOp", Float) = 0
    _StencilReadMask("StencilReadMask", Float) = 255
    _StencilWriteMask("StencilWriteMask", Float) = 255
    _ColorMask("ColorMask", Float) = 15
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }



        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
     ZTest[unity_GUIZTestMode]
    ZWrite Off

Stencil{
    Ref[_Stencil]
    Comp[_StencilComp]
    Pass[_StencilOp]
    ReadMask[_StencilReadMask]
    WriteMask[_StencilWriteMask]
}
ColorMask[_ColorMask]

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float4 texCoord0;
        float4 color;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 WorldSpaceTangent;
        float3 ObjectSpaceBiTangent;
        float3 WorldSpaceBiTangent;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float4 uv0;
        float3 TimeParameters;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float4 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyzw = input.texCoord0;
        output.interp1.xyzw = input.color;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.texCoord0 = input.interp0.xyzw;
        output.color = input.interp1.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float Vector1_4a9c697724064c6dbc765e7bc85f89c7;
float2 Vector2_49f7f77a33904423ad6b5bbc47d673aa;
float Vector1_dec963672df24678a19b17b8fef25c38;
float Vector1_2b12f4302b4e4c779cce0997b283c146;
float Vector1_e1ec7c947fa74423b9e01b6f1234c57b;
float Vector1_1a79ced4a0574d24b95f79288f00ec55;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Absolute_float(float In, out float Out)
{
    Out = abs(In);
}

void Unity_Power_float(float A, float B, out float Out)
{
    Out = pow(A, B);
}

void Unity_Clamp_float(float In, float Min, float Max, out float Out)
{
    Out = clamp(In, Min, Max);
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}


float2 Unity_GradientNoise_Dir_float(float2 p)
{
    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
    p = p % 289;
    // need full precision, otherwise half overflows when p > 1
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
{
    float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
    Out = luma.xxx + Saturation.xxx * (In - luma.xxx);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    float4 _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0 = IN.uv0;
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_R_1 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[0];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_G_2 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[1];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_B_3 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[2];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_A_4 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[3];
    float _Absolute_e29d170aaa464329839a8e13f38a0407_Out_1;
    Unity_Absolute_float(_Split_edafc542b11a4fc8b0efb7b8be04f6cd_G_2, _Absolute_e29d170aaa464329839a8e13f38a0407_Out_1);
    float _Property_e50720e3fdd7472b9f25a9515426f2cf_Out_0 = Vector1_e1ec7c947fa74423b9e01b6f1234c57b;
    float _Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2;
    Unity_Power_float(_Absolute_e29d170aaa464329839a8e13f38a0407_Out_1, _Property_e50720e3fdd7472b9f25a9515426f2cf_Out_0, _Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2);
    float _Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3;
    Unity_Clamp_float(_Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2, 0, 1, _Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3);
    float _Split_5ed28ba2b78b482abbe29c104db7b206_R_1 = IN.WorldSpacePosition[0];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_G_2 = IN.WorldSpacePosition[1];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_B_3 = IN.WorldSpacePosition[2];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_A_4 = 0;
    float2 _Vector2_2c590459ac2247d4afa44366ea74bd2a_Out_0 = float2(_Split_5ed28ba2b78b482abbe29c104db7b206_R_1, _Split_5ed28ba2b78b482abbe29c104db7b206_G_2);
    float2 _Property_8ef7ce0eeb2544309cb203c36c401965_Out_0 = Vector2_49f7f77a33904423ad6b5bbc47d673aa;
    float _Property_bdf12bfba62849818d977c0be8a4b045_Out_0 = Vector1_4a9c697724064c6dbc765e7bc85f89c7;
    float _Multiply_2e3c223d3e1345758aa85373d972c313_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_bdf12bfba62849818d977c0be8a4b045_Out_0, _Multiply_2e3c223d3e1345758aa85373d972c313_Out_2);
    float2 _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2;
    Unity_Multiply_float(_Property_8ef7ce0eeb2544309cb203c36c401965_Out_0, (_Multiply_2e3c223d3e1345758aa85373d972c313_Out_2.xx), _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2);
    float2 _TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3;
    Unity_TilingAndOffset_float(_Vector2_2c590459ac2247d4afa44366ea74bd2a_Out_0, float2 (1, 1), _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2, _TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3);
    float _Property_f533a7b3c6db4e7ba7a29e9c6320a0af_Out_0 = Vector1_dec963672df24678a19b17b8fef25c38;
    float _GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3, _Property_f533a7b3c6db4e7ba7a29e9c6320a0af_Out_0, _GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2);
    float _Add_2311ca51a9f74d27b06b1b4761302f94_Out_2;
    Unity_Add_float(_GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2, -0.5, _Add_2311ca51a9f74d27b06b1b4761302f94_Out_2);
    float _Property_104f65f819f3458a9f61ac7b48051b8c_Out_0 = Vector1_2b12f4302b4e4c779cce0997b283c146;
    float _Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2;
    Unity_Multiply_float(_Add_2311ca51a9f74d27b06b1b4761302f94_Out_2, _Property_104f65f819f3458a9f61ac7b48051b8c_Out_0, _Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2);
    float2 _Property_c21c0a8fc50145c2abf11bd667845da5_Out_0 = Vector2_49f7f77a33904423ad6b5bbc47d673aa;
    float2 _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2;
    Unity_Multiply_float((_Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2.xx), _Property_c21c0a8fc50145c2abf11bd667845da5_Out_0, _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2);
    float2 _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2;
    Unity_Multiply_float((_Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3.xx), _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2, _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2);
    float _Split_581d5dab3d2f47489277257672c54b28_R_1 = _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2[0];
    float _Split_581d5dab3d2f47489277257672c54b28_G_2 = _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2[1];
    float _Split_581d5dab3d2f47489277257672c54b28_B_3 = 0;
    float _Split_581d5dab3d2f47489277257672c54b28_A_4 = 0;
    float2 _Vector2_41f3260b04a24c3bb2c143d40a360b68_Out_0 = float2(_Split_581d5dab3d2f47489277257672c54b28_R_1, _Split_581d5dab3d2f47489277257672c54b28_G_2);
    float2 _Add_725473904669406f950bd5e7131a4ca4_Out_2;
    Unity_Add_float2(_Vector2_41f3260b04a24c3bb2c143d40a360b68_Out_0, (IN.WorldSpacePosition.xy), _Add_725473904669406f950bd5e7131a4ca4_Out_2);
    float3 _Transform_9e990f88f9ff4f689f93c4ba7aab5e96_Out_1 = TransformWorldToObject((float3(_Add_725473904669406f950bd5e7131a4ca4_Out_2, 0.0)).xyz);
    description.Position = _Transform_9e990f88f9ff4f689f93c4ba7aab5e96_Out_1;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_5a42b416109a4b4fbaad679d8aad4219_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5a42b416109a4b4fbaad679d8aad4219_Out_0.tex, _Property_5a42b416109a4b4fbaad679d8aad4219_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_R_4 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.r;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_G_5 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.g;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_B_6 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.b;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_A_7 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.a;
    float4 _Combine_0a32454a91514fce8e93e2cb25940325_RGBA_4;
    float3 _Combine_0a32454a91514fce8e93e2cb25940325_RGB_5;
    float2 _Combine_0a32454a91514fce8e93e2cb25940325_RG_6;
    Unity_Combine_float(_SampleTexture2D_00dffb92afe84c109276e37058096118_R_4, _SampleTexture2D_00dffb92afe84c109276e37058096118_G_5, _SampleTexture2D_00dffb92afe84c109276e37058096118_B_6, 0, _Combine_0a32454a91514fce8e93e2cb25940325_RGBA_4, _Combine_0a32454a91514fce8e93e2cb25940325_RGB_5, _Combine_0a32454a91514fce8e93e2cb25940325_RG_6);
    float _Property_10260d5b30da45a695e0d57836774a0b_Out_0 = Vector1_1a79ced4a0574d24b95f79288f00ec55;
    float3 _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2;
    Unity_Saturation_float(_Combine_0a32454a91514fce8e93e2cb25940325_RGB_5, _Property_10260d5b30da45a695e0d57836774a0b_Out_0, _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2);
    surface.BaseColor = _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2;
    surface.Alpha = _SampleTexture2D_00dffb92afe84c109276e37058096118_A_7;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.WorldSpaceNormal = TransformObjectToWorldNormal(input.normalOS);
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.WorldSpaceTangent = TransformObjectToWorldDir(input.tangentOS.xyz);
    output.ObjectSpaceBiTangent = normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
    output.WorldSpaceBiTangent = TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
    output.ObjectSpacePosition = input.positionOS;
    output.WorldSpacePosition = TransformObjectToWorld(input.positionOS);
    output.uv0 = input.uv0;
    output.TimeParameters = _TimeParameters.xyz;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Unlit"
    Tags
    {
        "LightMode" = "UniversalForward"
    }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float4 texCoord0;
        float4 color;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 WorldSpaceTangent;
        float3 ObjectSpaceBiTangent;
        float3 WorldSpaceBiTangent;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float4 uv0;
        float3 TimeParameters;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float4 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyzw = input.texCoord0;
        output.interp1.xyzw = input.color;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.texCoord0 = input.interp0.xyzw;
        output.color = input.interp1.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float Vector1_4a9c697724064c6dbc765e7bc85f89c7;
float2 Vector2_49f7f77a33904423ad6b5bbc47d673aa;
float Vector1_dec963672df24678a19b17b8fef25c38;
float Vector1_2b12f4302b4e4c779cce0997b283c146;
float Vector1_e1ec7c947fa74423b9e01b6f1234c57b;
float Vector1_1a79ced4a0574d24b95f79288f00ec55;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Absolute_float(float In, out float Out)
{
    Out = abs(In);
}

void Unity_Power_float(float A, float B, out float Out)
{
    Out = pow(A, B);
}

void Unity_Clamp_float(float In, float Min, float Max, out float Out)
{
    Out = clamp(In, Min, Max);
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}


float2 Unity_GradientNoise_Dir_float(float2 p)
{
    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
    p = p % 289;
    // need full precision, otherwise half overflows when p > 1
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
{
    float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
    Out = luma.xxx + Saturation.xxx * (In - luma.xxx);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    float4 _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0 = IN.uv0;
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_R_1 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[0];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_G_2 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[1];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_B_3 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[2];
    float _Split_edafc542b11a4fc8b0efb7b8be04f6cd_A_4 = _UV_a8358ebc673c46f1aa0721cb748a0d63_Out_0[3];
    float _Absolute_e29d170aaa464329839a8e13f38a0407_Out_1;
    Unity_Absolute_float(_Split_edafc542b11a4fc8b0efb7b8be04f6cd_G_2, _Absolute_e29d170aaa464329839a8e13f38a0407_Out_1);
    float _Property_e50720e3fdd7472b9f25a9515426f2cf_Out_0 = Vector1_e1ec7c947fa74423b9e01b6f1234c57b;
    float _Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2;
    Unity_Power_float(_Absolute_e29d170aaa464329839a8e13f38a0407_Out_1, _Property_e50720e3fdd7472b9f25a9515426f2cf_Out_0, _Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2);
    float _Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3;
    Unity_Clamp_float(_Power_e2d5a16a24344519b0d454fbc2e71e77_Out_2, 0, 1, _Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3);
    float _Split_5ed28ba2b78b482abbe29c104db7b206_R_1 = IN.WorldSpacePosition[0];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_G_2 = IN.WorldSpacePosition[1];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_B_3 = IN.WorldSpacePosition[2];
    float _Split_5ed28ba2b78b482abbe29c104db7b206_A_4 = 0;
    float2 _Vector2_2c590459ac2247d4afa44366ea74bd2a_Out_0 = float2(_Split_5ed28ba2b78b482abbe29c104db7b206_R_1, _Split_5ed28ba2b78b482abbe29c104db7b206_G_2);
    float2 _Property_8ef7ce0eeb2544309cb203c36c401965_Out_0 = Vector2_49f7f77a33904423ad6b5bbc47d673aa;
    float _Property_bdf12bfba62849818d977c0be8a4b045_Out_0 = Vector1_4a9c697724064c6dbc765e7bc85f89c7;
    float _Multiply_2e3c223d3e1345758aa85373d972c313_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_bdf12bfba62849818d977c0be8a4b045_Out_0, _Multiply_2e3c223d3e1345758aa85373d972c313_Out_2);
    float2 _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2;
    Unity_Multiply_float(_Property_8ef7ce0eeb2544309cb203c36c401965_Out_0, (_Multiply_2e3c223d3e1345758aa85373d972c313_Out_2.xx), _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2);
    float2 _TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3;
    Unity_TilingAndOffset_float(_Vector2_2c590459ac2247d4afa44366ea74bd2a_Out_0, float2 (1, 1), _Multiply_b51aa9f32c2246dc9183db6b5c5ad5b6_Out_2, _TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3);
    float _Property_f533a7b3c6db4e7ba7a29e9c6320a0af_Out_0 = Vector1_dec963672df24678a19b17b8fef25c38;
    float _GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_2e11605ab48a430cb3aaf4cd2296c9b7_Out_3, _Property_f533a7b3c6db4e7ba7a29e9c6320a0af_Out_0, _GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2);
    float _Add_2311ca51a9f74d27b06b1b4761302f94_Out_2;
    Unity_Add_float(_GradientNoise_115f55e3423040a7b2979d0d1a7d442b_Out_2, -0.5, _Add_2311ca51a9f74d27b06b1b4761302f94_Out_2);
    float _Property_104f65f819f3458a9f61ac7b48051b8c_Out_0 = Vector1_2b12f4302b4e4c779cce0997b283c146;
    float _Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2;
    Unity_Multiply_float(_Add_2311ca51a9f74d27b06b1b4761302f94_Out_2, _Property_104f65f819f3458a9f61ac7b48051b8c_Out_0, _Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2);
    float2 _Property_c21c0a8fc50145c2abf11bd667845da5_Out_0 = Vector2_49f7f77a33904423ad6b5bbc47d673aa;
    float2 _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2;
    Unity_Multiply_float((_Multiply_11e715298c1e4e98aa4e8b5e13c82f8a_Out_2.xx), _Property_c21c0a8fc50145c2abf11bd667845da5_Out_0, _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2);
    float2 _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2;
    Unity_Multiply_float((_Clamp_3d31ee7d8ff44e3e950d8535a26b67bc_Out_3.xx), _Multiply_5590a7f667c5478ab752bb190cdf804c_Out_2, _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2);
    float _Split_581d5dab3d2f47489277257672c54b28_R_1 = _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2[0];
    float _Split_581d5dab3d2f47489277257672c54b28_G_2 = _Multiply_006a470a5e01478c8ee7f763c388eb4c_Out_2[1];
    float _Split_581d5dab3d2f47489277257672c54b28_B_3 = 0;
    float _Split_581d5dab3d2f47489277257672c54b28_A_4 = 0;
    float2 _Vector2_41f3260b04a24c3bb2c143d40a360b68_Out_0 = float2(_Split_581d5dab3d2f47489277257672c54b28_R_1, _Split_581d5dab3d2f47489277257672c54b28_G_2);
    float2 _Add_725473904669406f950bd5e7131a4ca4_Out_2;
    Unity_Add_float2(_Vector2_41f3260b04a24c3bb2c143d40a360b68_Out_0, (IN.WorldSpacePosition.xy), _Add_725473904669406f950bd5e7131a4ca4_Out_2);
    float3 _Transform_9e990f88f9ff4f689f93c4ba7aab5e96_Out_1 = TransformWorldToObject((float3(_Add_725473904669406f950bd5e7131a4ca4_Out_2, 0.0)).xyz);
    description.Position = _Transform_9e990f88f9ff4f689f93c4ba7aab5e96_Out_1;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_5a42b416109a4b4fbaad679d8aad4219_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5a42b416109a4b4fbaad679d8aad4219_Out_0.tex, _Property_5a42b416109a4b4fbaad679d8aad4219_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_R_4 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.r;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_G_5 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.g;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_B_6 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.b;
    float _SampleTexture2D_00dffb92afe84c109276e37058096118_A_7 = _SampleTexture2D_00dffb92afe84c109276e37058096118_RGBA_0.a;
    float4 _Combine_0a32454a91514fce8e93e2cb25940325_RGBA_4;
    float3 _Combine_0a32454a91514fce8e93e2cb25940325_RGB_5;
    float2 _Combine_0a32454a91514fce8e93e2cb25940325_RG_6;
    Unity_Combine_float(_SampleTexture2D_00dffb92afe84c109276e37058096118_R_4, _SampleTexture2D_00dffb92afe84c109276e37058096118_G_5, _SampleTexture2D_00dffb92afe84c109276e37058096118_B_6, 0, _Combine_0a32454a91514fce8e93e2cb25940325_RGBA_4, _Combine_0a32454a91514fce8e93e2cb25940325_RGB_5, _Combine_0a32454a91514fce8e93e2cb25940325_RG_6);
    float _Property_10260d5b30da45a695e0d57836774a0b_Out_0 = Vector1_1a79ced4a0574d24b95f79288f00ec55;
    float3 _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2;
    Unity_Saturation_float(_Combine_0a32454a91514fce8e93e2cb25940325_RGB_5, _Property_10260d5b30da45a695e0d57836774a0b_Out_0, _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2);
    surface.BaseColor = _Saturation_ec692e186cc64bb3ae22882399078dae_Out_2;
    surface.Alpha = _SampleTexture2D_00dffb92afe84c109276e37058096118_A_7;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.WorldSpaceNormal = TransformObjectToWorldNormal(input.normalOS);
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.WorldSpaceTangent = TransformObjectToWorldDir(input.tangentOS.xyz);
    output.ObjectSpaceBiTangent = normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
    output.WorldSpaceBiTangent = TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
    output.ObjectSpacePosition = input.positionOS;
    output.WorldSpacePosition = TransformObjectToWorld(input.positionOS);
    output.uv0 = input.uv0;
    output.TimeParameters = _TimeParameters.xyz;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}