Shader "RulletWaterShader"
{
    Properties
    {
        [NoScaleOffset] _NormalTex("NormalTex", 2D) = "white" {}
        Vector2_de119d2fb17a4aa98948b0f68f98e925("PositionOffset", Vector) = (0.05, -0.05, 0, 0)
        Vector2_47cdc3f1111d478b850a2faa8dfe283d("NormalDirection", Vector) = (0, 0, 0, 0)
        Vector1_ac125a03065d45ea825275174371e270("NormalMapMoveSpeed", Float) = 1
        Vector2_26891710b908471c9595ba6d4c024ace("NoiseDir", Vector) = (-0.5, 0.5, 0, 0)
        [HDR]Color_1d6caaa9227e48b7aa0b9cea86f1fed3("NoiseColor", Color) = (0.5209134, 2.015213, 2.007389, 0)
        Vector1_18fbdcd160d0493b8d24210610ca41b8("WaterSpeed", Float) = 0.2
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        Vector1_e4917ee38d6847ec9dc4ffbc023e8bfb("ShaderAlpha", Float) = 0.5
        [HDR]Color_d45af51239fd4f65944fe1d574db43f3("OriginColor", Color) = (0, 0.5529412, 1, 0)
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
            "UniversalMaterialType" = "Lit"
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
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

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
        float4 screenPosition;
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
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
        float3 TimeParameters;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float4 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
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
        output.interp2.xyzw = input.screenPosition;
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
        output.screenPosition = input.interp2.xyzw;
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
float4 _NormalTex_TexelSize;
float2 Vector2_de119d2fb17a4aa98948b0f68f98e925;
float2 Vector2_47cdc3f1111d478b850a2faa8dfe283d;
float Vector1_ac125a03065d45ea825275174371e270;
float2 Vector2_26891710b908471c9595ba6d4c024ace;
float4 Color_1d6caaa9227e48b7aa0b9cea86f1fed3;
float Vector1_18fbdcd160d0493b8d24210610ca41b8;
float4 _MainTex_TexelSize;
float Vector1_e4917ee38d6847ec9dc4ffbc023e8bfb;
float4 Color_d45af51239fd4f65944fe1d574db43f3;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_NormalTex);
SAMPLER(sampler_NormalTex);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
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
    float2 _Property_69872e0a33be44cd82d78bed82915f9d_Out_0 = Vector2_de119d2fb17a4aa98948b0f68f98e925;
    float2 _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2;
    Unity_Multiply_float((IN.TimeParameters.y.xx), _Property_69872e0a33be44cd82d78bed82915f9d_Out_0, _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2);
    float2 _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2;
    Unity_Add_float2((IN.ObjectSpacePosition.xy), _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2, _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2);
    description.Position = (float3(_Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2, 0.0));
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float4 SpriteMask;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.tex, _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.r;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.g;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.b;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.a;
    float4 _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4;
    float3 _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    float2 _Combine_7e23858362e34f3fa98ce264e1208067_RG_6;
    Unity_Combine_float(_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6, 0, _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Combine_7e23858362e34f3fa98ce264e1208067_RG_6);
    surface.BaseColor = _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    surface.Alpha = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7;
    surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;
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
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Normal"
    Tags
    {
        "LightMode" = "NormalsRendering"
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
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITENORMAL
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 normalWS;
        float4 tangentWS;
        float4 texCoord0;
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
        float3 TangentSpaceNormal;
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
        float3 TimeParameters;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
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
        output.interp0.xyz = input.normalWS;
        output.interp1.xyzw = input.tangentWS;
        output.interp2.xyzw = input.texCoord0;
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
        output.normalWS = input.interp0.xyz;
        output.tangentWS = input.interp1.xyzw;
        output.texCoord0 = input.interp2.xyzw;
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
float4 _NormalTex_TexelSize;
float2 Vector2_de119d2fb17a4aa98948b0f68f98e925;
float2 Vector2_47cdc3f1111d478b850a2faa8dfe283d;
float Vector1_ac125a03065d45ea825275174371e270;
float2 Vector2_26891710b908471c9595ba6d4c024ace;
float4 Color_1d6caaa9227e48b7aa0b9cea86f1fed3;
float Vector1_18fbdcd160d0493b8d24210610ca41b8;
float4 _MainTex_TexelSize;
float Vector1_e4917ee38d6847ec9dc4ffbc023e8bfb;
float4 Color_d45af51239fd4f65944fe1d574db43f3;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_NormalTex);
SAMPLER(sampler_NormalTex);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
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
    float2 _Property_69872e0a33be44cd82d78bed82915f9d_Out_0 = Vector2_de119d2fb17a4aa98948b0f68f98e925;
    float2 _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2;
    Unity_Multiply_float((IN.TimeParameters.y.xx), _Property_69872e0a33be44cd82d78bed82915f9d_Out_0, _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2);
    float2 _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2;
    Unity_Add_float2((IN.ObjectSpacePosition.xy), _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2, _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2);
    description.Position = (float3(_Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2, 0.0));
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.tex, _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.r;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.g;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.b;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.a;
    float4 _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4;
    float3 _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    float2 _Combine_7e23858362e34f3fa98ce264e1208067_RG_6;
    Unity_Combine_float(_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6, 0, _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Combine_7e23858362e34f3fa98ce264e1208067_RG_6);
    surface.BaseColor = _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    surface.Alpha = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;
    output.TimeParameters = _TimeParameters.xyz;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


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
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Forward"
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
        float3 TangentSpaceNormal;
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
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
float4 _NormalTex_TexelSize;
float2 Vector2_de119d2fb17a4aa98948b0f68f98e925;
float2 Vector2_47cdc3f1111d478b850a2faa8dfe283d;
float Vector1_ac125a03065d45ea825275174371e270;
float2 Vector2_26891710b908471c9595ba6d4c024ace;
float4 Color_1d6caaa9227e48b7aa0b9cea86f1fed3;
float Vector1_18fbdcd160d0493b8d24210610ca41b8;
float4 _MainTex_TexelSize;
float Vector1_e4917ee38d6847ec9dc4ffbc023e8bfb;
float4 Color_d45af51239fd4f65944fe1d574db43f3;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_NormalTex);
SAMPLER(sampler_NormalTex);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
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
    float2 _Property_69872e0a33be44cd82d78bed82915f9d_Out_0 = Vector2_de119d2fb17a4aa98948b0f68f98e925;
    float2 _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2;
    Unity_Multiply_float((IN.TimeParameters.y.xx), _Property_69872e0a33be44cd82d78bed82915f9d_Out_0, _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2);
    float2 _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2;
    Unity_Add_float2((IN.ObjectSpacePosition.xy), _Multiply_59b83c98bf8c4097939a7fdd81ce5a36_Out_2, _Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2);
    description.Position = (float3(_Add_3a74bc1e842c48a8b90ffeadaaf1a507_Out_2, 0.0));
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.tex, _Property_a73451c20b9641ff9862d74cc55ed8bf_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.r;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.g;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.b;
    float _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7 = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_RGBA_0.a;
    float4 _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4;
    float3 _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    float2 _Combine_7e23858362e34f3fa98ce264e1208067_RG_6;
    Unity_Combine_float(_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6, 0, _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Combine_7e23858362e34f3fa98ce264e1208067_RG_6);
    surface.BaseColor = _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    surface.Alpha = _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;
    output.TimeParameters = _TimeParameters.xyz;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


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
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}