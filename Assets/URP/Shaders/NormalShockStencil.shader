Shader "RulletNormalShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        Vector2_29aacd965df341caa3a51411131f3cf7("FocalPoint", Vector) = (0.5, 0.5, 0, 0)
        Vector1_4af17b01eb504bcd85e9bc9d6322cfc5("Size", Float) = 0.12
        Vector1_fd842caa074042a39dec2b7ed7b62974("Magnification", Float) = 1
        Vector1_02baa6d791b14d088c76eac03ec5aa0a("ShockIntensity", Float) = 1
        Vector1_a23e2a40cac3433f8271428bf2f68219("Speed", Float) = 1
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
            Name "Sprite Lit"
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
        float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
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
float4 _MainTex_TexelSize;
float2 Vector2_29aacd965df341caa3a51411131f3cf7;
float Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
float Vector1_fd842caa074042a39dec2b7ed7b62974;
float Vector1_02baa6d791b14d088c76eac03ec5aa0a;
float Vector1_a23e2a40cac3433f8271428bf2f68219;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Fraction_float(float In, out float Out)
{
    Out = frac(In);
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
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
    description.Position = IN.ObjectSpacePosition;
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
    UnityTexture2D _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float _Property_6d33129cc957489b90787dd84e27afe4_Out_0 = Vector1_a23e2a40cac3433f8271428bf2f68219;
    float _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_6d33129cc957489b90787dd84e27afe4_Out_0, _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2);
    float _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1;
    Unity_Fraction_float(_Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2, _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1);
    float _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0 = Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
    float _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2;
    Unity_Subtract_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2);
    float _Add_b368bdbc003d4b4e9eda93b742375069_Out_2;
    Unity_Add_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2);
    float4 _UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0 = IN.uv0;
    float2 _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0 = Vector2_29aacd965df341caa3a51411131f3cf7;
    float2 _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2;
    Unity_Subtract_float2((_UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0.xy), _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0, _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2);
    float _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1;
    Unity_Length_float2(_Subtract_b45850f6e5f34861add08a8f897780ca_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1);
    float _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3;
    Unity_Smoothstep_float(_Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3);
    float _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2;
    Unity_Add_float(_Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2);
    float4 _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.tex, _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.samplerstate, (_Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2.xx));
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.r;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.g;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.b;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.a;
    float4 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4;
    float3 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    float2 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6;
    Unity_Combine_float(_SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6, 0, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6);
    surface.BaseColor = _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    surface.Alpha = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7;
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

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
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
        float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
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
float4 _MainTex_TexelSize;
float2 Vector2_29aacd965df341caa3a51411131f3cf7;
float Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
float Vector1_fd842caa074042a39dec2b7ed7b62974;
float Vector1_02baa6d791b14d088c76eac03ec5aa0a;
float Vector1_a23e2a40cac3433f8271428bf2f68219;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Fraction_float(float In, out float Out)
{
    Out = frac(In);
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
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
    description.Position = IN.ObjectSpacePosition;
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
    UnityTexture2D _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float _Property_6d33129cc957489b90787dd84e27afe4_Out_0 = Vector1_a23e2a40cac3433f8271428bf2f68219;
    float _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_6d33129cc957489b90787dd84e27afe4_Out_0, _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2);
    float _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1;
    Unity_Fraction_float(_Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2, _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1);
    float _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0 = Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
    float _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2;
    Unity_Subtract_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2);
    float _Add_b368bdbc003d4b4e9eda93b742375069_Out_2;
    Unity_Add_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2);
    float4 _UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0 = IN.uv0;
    float2 _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0 = Vector2_29aacd965df341caa3a51411131f3cf7;
    float2 _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2;
    Unity_Subtract_float2((_UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0.xy), _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0, _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2);
    float _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1;
    Unity_Length_float2(_Subtract_b45850f6e5f34861add08a8f897780ca_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1);
    float _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3;
    Unity_Smoothstep_float(_Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3);
    float _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2;
    Unity_Add_float(_Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2);
    float4 _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.tex, _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.samplerstate, (_Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2.xx));
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.r;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.g;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.b;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.a;
    float4 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4;
    float3 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    float2 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6;
    Unity_Combine_float(_SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6, 0, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6);
    surface.BaseColor = _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    surface.Alpha = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7;
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

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
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
        float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
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
float2 Vector2_29aacd965df341caa3a51411131f3cf7;
float Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
float Vector1_fd842caa074042a39dec2b7ed7b62974;
float Vector1_02baa6d791b14d088c76eac03ec5aa0a;
float Vector1_a23e2a40cac3433f8271428bf2f68219;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Fraction_float(float In, out float Out)
{
    Out = frac(In);
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
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
    description.Position = IN.ObjectSpacePosition;
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
    UnityTexture2D _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float _Property_6d33129cc957489b90787dd84e27afe4_Out_0 = Vector1_a23e2a40cac3433f8271428bf2f68219;
    float _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_6d33129cc957489b90787dd84e27afe4_Out_0, _Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2);
    float _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1;
    Unity_Fraction_float(_Multiply_e0736d98156b4624bf039700ee0bc9ef_Out_2, _Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1);
    float _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0 = Vector1_4af17b01eb504bcd85e9bc9d6322cfc5;
    float _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2;
    Unity_Subtract_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2);
    float _Add_b368bdbc003d4b4e9eda93b742375069_Out_2;
    Unity_Add_float(_Fraction_b8130a03ca5b41348aae6c52c6b8d1fe_Out_1, _Property_87c09ccf02654da0b4e3cd569b838be4_Out_0, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2);
    float4 _UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0 = IN.uv0;
    float2 _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0 = Vector2_29aacd965df341caa3a51411131f3cf7;
    float2 _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2;
    Unity_Subtract_float2((_UV_ddf5270b5a6b47ec8d2d6640e2d1d472_Out_0.xy), _Property_72edd2ee6d2d4e54bd1d275ae0296bd3_Out_0, _Subtract_b45850f6e5f34861add08a8f897780ca_Out_2);
    float _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1;
    Unity_Length_float2(_Subtract_b45850f6e5f34861add08a8f897780ca_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1);
    float _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3;
    Unity_Smoothstep_float(_Subtract_2b070135eb714a7d91d91e268fb88a18_Out_2, _Add_b368bdbc003d4b4e9eda93b742375069_Out_2, _Length_b9a4a8b6864d4ecdaf8b8b4b33d592dd_Out_1, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3);
    float _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2;
    Unity_Add_float(_Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Smoothstep_4be02f70b59845559c9da025b981cab0_Out_3, _Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2);
    float4 _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.tex, _Property_4b6f9700d8a14057b51de3a377dd0fed_Out_0.samplerstate, (_Add_489584a432b24a0cb6cc6e6a3188d1d5_Out_2.xx));
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.r;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.g;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.b;
    float _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7 = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_RGBA_0.a;
    float4 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4;
    float3 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    float2 _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6;
    Unity_Combine_float(_SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_R_4, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_G_5, _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_B_6, 0, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGBA_4, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5, _Combine_d05b24bde6c74749be602e9fcc5a8efa_RG_6);
    surface.BaseColor = _Combine_d05b24bde6c74749be602e9fcc5a8efa_RGB_5;
    surface.Alpha = _SampleTexture2D_829a3c0e1a7b44db8d3327123e706803_A_7;
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

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
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