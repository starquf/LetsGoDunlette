Shader "RulletHeatShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        Vector1_034eca248f2743288fed574c5d4cc21c("TimeSpeed", Range(0, 1)) = 0.25
        Vector1_cee06d300dcb4d80b6047ff0c6bc50d8("TimeOffset", Range(10, 100)) = 40
        Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec("HazeStrength", Range(0.01, 1)) = 0.01
        Vector1_3("Speed", Float) = 0.125
        Vector1_4("Celldensity", Float) = 50
        Vector1_1("AngleOffset", Float) = 30
        Vector1_2("AngleSpeed", Float) = 10
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
float Vector1_034eca248f2743288fed574c5d4cc21c;
float Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
float Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
float Vector1_3;
float Vector1_4;
float Vector1_1;
float Vector1_2;
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

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}


inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x,y);
            float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);

            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
        }
    }
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
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
    UnityTexture2D _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _UV_720332d0b58a41838dcda3783f7655a5_Out_0 = IN.uv0;
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_R_1 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[0];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[1];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_B_3 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[2];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_A_4 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[3];
    float _Property_24f29c7f319c4146b28ba21c95e6d127_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float4 _UV_791c5ae425b043b4aa37af24863276d5_Out_0 = IN.uv0;
    float _Split_0418d2892f07470fba19f9b50d058d6a_R_1 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[0];
    float _Split_0418d2892f07470fba19f9b50d058d6a_G_2 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[1];
    float _Split_0418d2892f07470fba19f9b50d058d6a_B_3 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[2];
    float _Split_0418d2892f07470fba19f9b50d058d6a_A_4 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[3];
    float _Property_bebac6a81b32484c8f0c85b789d7c317_Out_0 = Vector1_3;
    float _Multiply_565a113d81164ab2b010889636a707a1_Out_2;
    Unity_Multiply_float(_Property_bebac6a81b32484c8f0c85b789d7c317_Out_0, IN.TimeParameters.x, _Multiply_565a113d81164ab2b010889636a707a1_Out_2);
    float _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2;
    Unity_Subtract_float(_Split_0418d2892f07470fba19f9b50d058d6a_G_2, _Multiply_565a113d81164ab2b010889636a707a1_Out_2, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2);
    float4 _Combine_049933412b4243e98a010979bafaefd1_RGBA_4;
    float3 _Combine_049933412b4243e98a010979bafaefd1_RGB_5;
    float2 _Combine_049933412b4243e98a010979bafaefd1_RG_6;
    Unity_Combine_float(_Split_0418d2892f07470fba19f9b50d058d6a_R_1, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2, 0, 0, _Combine_049933412b4243e98a010979bafaefd1_RGBA_4, _Combine_049933412b4243e98a010979bafaefd1_RGB_5, _Combine_049933412b4243e98a010979bafaefd1_RG_6);
    float _Property_85c2f79046e6400887a5d99f228b81c0_Out_0 = Vector1_1;
    float _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0 = Vector1_2;
    float _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2);
    float _Add_de1cb74da73a497882044e8f09c674f5_Out_2;
    Unity_Add_float(_Property_85c2f79046e6400887a5d99f228b81c0_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2, _Add_de1cb74da73a497882044e8f09c674f5_Out_2);
    float _Property_aac3be73cb6c484da988686d3292b4da_Out_0 = Vector1_4;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4;
    Unity_Voronoi_float(_Combine_049933412b4243e98a010979bafaefd1_RG_6, _Add_de1cb74da73a497882044e8f09c674f5_Out_2, _Property_aac3be73cb6c484da988686d3292b4da_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4);
    float _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2;
    Unity_Multiply_float(_Property_24f29c7f319c4146b28ba21c95e6d127_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2);
    float _Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float _Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0 = Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
    float _Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0 = Vector1_034eca248f2743288fed574c5d4cc21c;
    float _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2;
    Unity_Multiply_float(_Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0, IN.TimeParameters.x, _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2);
    float _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2;
    Unity_Subtract_float(_Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2);
    float _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2;
    Unity_Multiply_float(_Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2, _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2);
    float _Sine_0c2299e27cfe445484760bf49660face_Out_1;
    Unity_Sine_float(_Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2, _Sine_0c2299e27cfe445484760bf49660face_Out_1);
    float _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2;
    Unity_Multiply_float(_Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0, _Sine_0c2299e27cfe445484760bf49660face_Out_1, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2);
    float _Add_e4dc466801924a7c95a16d64e652116b_Out_2;
    Unity_Add_float(_Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2, _Add_e4dc466801924a7c95a16d64e652116b_Out_2);
    float _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2;
    Unity_Add_float(_Split_24a1edaecbb248d380623d1f4bf71aaa_R_1, _Add_e4dc466801924a7c95a16d64e652116b_Out_2, _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2);
    float4 _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4;
    float3 _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5;
    float2 _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6;
    Unity_Combine_float(_Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, 0, 0, _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4, _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float4 _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0 = SAMPLE_TEXTURE2D(_Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.tex, _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.samplerstate, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.r;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.g;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.b;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.a;
    float4 _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4;
    float3 _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    float2 _Combine_37a060ee33194317a2ff054f74488ba3_RG_6;
    Unity_Combine_float(_SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4, _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5, _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6, 0, _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4, _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5, _Combine_37a060ee33194317a2ff054f74488ba3_RG_6);
    surface.BaseColor = _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    surface.Alpha = _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7;
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
float Vector1_034eca248f2743288fed574c5d4cc21c;
float Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
float Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
float Vector1_3;
float Vector1_4;
float Vector1_1;
float Vector1_2;
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

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}


inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x,y);
            float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);

            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
        }
    }
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
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
    UnityTexture2D _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _UV_720332d0b58a41838dcda3783f7655a5_Out_0 = IN.uv0;
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_R_1 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[0];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[1];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_B_3 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[2];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_A_4 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[3];
    float _Property_24f29c7f319c4146b28ba21c95e6d127_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float4 _UV_791c5ae425b043b4aa37af24863276d5_Out_0 = IN.uv0;
    float _Split_0418d2892f07470fba19f9b50d058d6a_R_1 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[0];
    float _Split_0418d2892f07470fba19f9b50d058d6a_G_2 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[1];
    float _Split_0418d2892f07470fba19f9b50d058d6a_B_3 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[2];
    float _Split_0418d2892f07470fba19f9b50d058d6a_A_4 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[3];
    float _Property_bebac6a81b32484c8f0c85b789d7c317_Out_0 = Vector1_3;
    float _Multiply_565a113d81164ab2b010889636a707a1_Out_2;
    Unity_Multiply_float(_Property_bebac6a81b32484c8f0c85b789d7c317_Out_0, IN.TimeParameters.x, _Multiply_565a113d81164ab2b010889636a707a1_Out_2);
    float _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2;
    Unity_Subtract_float(_Split_0418d2892f07470fba19f9b50d058d6a_G_2, _Multiply_565a113d81164ab2b010889636a707a1_Out_2, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2);
    float4 _Combine_049933412b4243e98a010979bafaefd1_RGBA_4;
    float3 _Combine_049933412b4243e98a010979bafaefd1_RGB_5;
    float2 _Combine_049933412b4243e98a010979bafaefd1_RG_6;
    Unity_Combine_float(_Split_0418d2892f07470fba19f9b50d058d6a_R_1, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2, 0, 0, _Combine_049933412b4243e98a010979bafaefd1_RGBA_4, _Combine_049933412b4243e98a010979bafaefd1_RGB_5, _Combine_049933412b4243e98a010979bafaefd1_RG_6);
    float _Property_85c2f79046e6400887a5d99f228b81c0_Out_0 = Vector1_1;
    float _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0 = Vector1_2;
    float _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2);
    float _Add_de1cb74da73a497882044e8f09c674f5_Out_2;
    Unity_Add_float(_Property_85c2f79046e6400887a5d99f228b81c0_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2, _Add_de1cb74da73a497882044e8f09c674f5_Out_2);
    float _Property_aac3be73cb6c484da988686d3292b4da_Out_0 = Vector1_4;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4;
    Unity_Voronoi_float(_Combine_049933412b4243e98a010979bafaefd1_RG_6, _Add_de1cb74da73a497882044e8f09c674f5_Out_2, _Property_aac3be73cb6c484da988686d3292b4da_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4);
    float _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2;
    Unity_Multiply_float(_Property_24f29c7f319c4146b28ba21c95e6d127_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2);
    float _Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float _Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0 = Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
    float _Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0 = Vector1_034eca248f2743288fed574c5d4cc21c;
    float _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2;
    Unity_Multiply_float(_Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0, IN.TimeParameters.x, _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2);
    float _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2;
    Unity_Subtract_float(_Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2);
    float _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2;
    Unity_Multiply_float(_Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2, _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2);
    float _Sine_0c2299e27cfe445484760bf49660face_Out_1;
    Unity_Sine_float(_Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2, _Sine_0c2299e27cfe445484760bf49660face_Out_1);
    float _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2;
    Unity_Multiply_float(_Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0, _Sine_0c2299e27cfe445484760bf49660face_Out_1, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2);
    float _Add_e4dc466801924a7c95a16d64e652116b_Out_2;
    Unity_Add_float(_Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2, _Add_e4dc466801924a7c95a16d64e652116b_Out_2);
    float _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2;
    Unity_Add_float(_Split_24a1edaecbb248d380623d1f4bf71aaa_R_1, _Add_e4dc466801924a7c95a16d64e652116b_Out_2, _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2);
    float4 _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4;
    float3 _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5;
    float2 _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6;
    Unity_Combine_float(_Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, 0, 0, _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4, _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float4 _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0 = SAMPLE_TEXTURE2D(_Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.tex, _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.samplerstate, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.r;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.g;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.b;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.a;
    float4 _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4;
    float3 _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    float2 _Combine_37a060ee33194317a2ff054f74488ba3_RG_6;
    Unity_Combine_float(_SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4, _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5, _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6, 0, _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4, _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5, _Combine_37a060ee33194317a2ff054f74488ba3_RG_6);
    surface.BaseColor = _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    surface.Alpha = _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7;
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
float Vector1_034eca248f2743288fed574c5d4cc21c;
float Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
float Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
float Vector1_3;
float Vector1_4;
float Vector1_1;
float Vector1_2;
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

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}


inline float2 Unity_Voronoi_RandomVector_float(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x,y);
            float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);

            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
        }
    }
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
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
    UnityTexture2D _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _UV_720332d0b58a41838dcda3783f7655a5_Out_0 = IN.uv0;
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_R_1 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[0];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[1];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_B_3 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[2];
    float _Split_24a1edaecbb248d380623d1f4bf71aaa_A_4 = _UV_720332d0b58a41838dcda3783f7655a5_Out_0[3];
    float _Property_24f29c7f319c4146b28ba21c95e6d127_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float4 _UV_791c5ae425b043b4aa37af24863276d5_Out_0 = IN.uv0;
    float _Split_0418d2892f07470fba19f9b50d058d6a_R_1 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[0];
    float _Split_0418d2892f07470fba19f9b50d058d6a_G_2 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[1];
    float _Split_0418d2892f07470fba19f9b50d058d6a_B_3 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[2];
    float _Split_0418d2892f07470fba19f9b50d058d6a_A_4 = _UV_791c5ae425b043b4aa37af24863276d5_Out_0[3];
    float _Property_bebac6a81b32484c8f0c85b789d7c317_Out_0 = Vector1_3;
    float _Multiply_565a113d81164ab2b010889636a707a1_Out_2;
    Unity_Multiply_float(_Property_bebac6a81b32484c8f0c85b789d7c317_Out_0, IN.TimeParameters.x, _Multiply_565a113d81164ab2b010889636a707a1_Out_2);
    float _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2;
    Unity_Subtract_float(_Split_0418d2892f07470fba19f9b50d058d6a_G_2, _Multiply_565a113d81164ab2b010889636a707a1_Out_2, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2);
    float4 _Combine_049933412b4243e98a010979bafaefd1_RGBA_4;
    float3 _Combine_049933412b4243e98a010979bafaefd1_RGB_5;
    float2 _Combine_049933412b4243e98a010979bafaefd1_RG_6;
    Unity_Combine_float(_Split_0418d2892f07470fba19f9b50d058d6a_R_1, _Subtract_7136b723d6b64bf0a8ba03d9f929860a_Out_2, 0, 0, _Combine_049933412b4243e98a010979bafaefd1_RGBA_4, _Combine_049933412b4243e98a010979bafaefd1_RGB_5, _Combine_049933412b4243e98a010979bafaefd1_RG_6);
    float _Property_85c2f79046e6400887a5d99f228b81c0_Out_0 = Vector1_1;
    float _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0 = Vector1_2;
    float _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_e7a1549838da49788ec9cbf6ffe9799b_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2);
    float _Add_de1cb74da73a497882044e8f09c674f5_Out_2;
    Unity_Add_float(_Property_85c2f79046e6400887a5d99f228b81c0_Out_0, _Multiply_e1c62838ed6d4accab089b914b0d212f_Out_2, _Add_de1cb74da73a497882044e8f09c674f5_Out_2);
    float _Property_aac3be73cb6c484da988686d3292b4da_Out_0 = Vector1_4;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3;
    float _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4;
    Unity_Voronoi_float(_Combine_049933412b4243e98a010979bafaefd1_RG_6, _Add_de1cb74da73a497882044e8f09c674f5_Out_2, _Property_aac3be73cb6c484da988686d3292b4da_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Cells_4);
    float _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2;
    Unity_Multiply_float(_Property_24f29c7f319c4146b28ba21c95e6d127_Out_0, _Voronoi_6ae0a4bd4aca4835acc6a751a3494e19_Out_3, _Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2);
    float _Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0 = Vector1_d13e41f4e5c44aa68f544bf0ce93e2ec;
    float _Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0 = Vector1_cee06d300dcb4d80b6047ff0c6bc50d8;
    float _Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0 = Vector1_034eca248f2743288fed574c5d4cc21c;
    float _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2;
    Unity_Multiply_float(_Property_f21af04b7ebf4cefb64a63c04e04f9ec_Out_0, IN.TimeParameters.x, _Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2);
    float _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2;
    Unity_Subtract_float(_Multiply_ce38bc8371534129b6677101ab4e3eb9_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2);
    float _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2;
    Unity_Multiply_float(_Property_4f4de2259ef94b96ae299386c8ff9e7c_Out_0, _Subtract_6226010bacf749c3987e50ee3aa38a51_Out_2, _Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2);
    float _Sine_0c2299e27cfe445484760bf49660face_Out_1;
    Unity_Sine_float(_Multiply_8948cbfaed2e448aad4b5f7a6826354e_Out_2, _Sine_0c2299e27cfe445484760bf49660face_Out_1);
    float _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2;
    Unity_Multiply_float(_Property_0007c84d38e348f788fb01f1ee8b61fe_Out_0, _Sine_0c2299e27cfe445484760bf49660face_Out_1, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2);
    float _Add_e4dc466801924a7c95a16d64e652116b_Out_2;
    Unity_Add_float(_Multiply_9e89a2a9f9df4edfaddbd3284132a8b7_Out_2, _Multiply_400e123268224238bf3b4e6a3d05d992_Out_2, _Add_e4dc466801924a7c95a16d64e652116b_Out_2);
    float _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2;
    Unity_Add_float(_Split_24a1edaecbb248d380623d1f4bf71aaa_R_1, _Add_e4dc466801924a7c95a16d64e652116b_Out_2, _Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2);
    float4 _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4;
    float3 _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5;
    float2 _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6;
    Unity_Combine_float(_Add_932b2a1a7e484d1a8ceaeda5e21c1d1d_Out_2, _Split_24a1edaecbb248d380623d1f4bf71aaa_G_2, 0, 0, _Combine_19f722cebaaa4a3fabc88092388b393f_RGBA_4, _Combine_19f722cebaaa4a3fabc88092388b393f_RGB_5, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float4 _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0 = SAMPLE_TEXTURE2D(_Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.tex, _Property_33e246a425aa49f8b73eacddd8f1d491_Out_0.samplerstate, _Combine_19f722cebaaa4a3fabc88092388b393f_RG_6);
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.r;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.g;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.b;
    float _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7 = _SampleTexture2D_52371273896446f2a10b7b546ac49482_RGBA_0.a;
    float4 _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4;
    float3 _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    float2 _Combine_37a060ee33194317a2ff054f74488ba3_RG_6;
    Unity_Combine_float(_SampleTexture2D_52371273896446f2a10b7b546ac49482_R_4, _SampleTexture2D_52371273896446f2a10b7b546ac49482_G_5, _SampleTexture2D_52371273896446f2a10b7b546ac49482_B_6, 0, _Combine_37a060ee33194317a2ff054f74488ba3_RGBA_4, _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5, _Combine_37a060ee33194317a2ff054f74488ba3_RG_6);
    surface.BaseColor = _Combine_37a060ee33194317a2ff054f74488ba3_RGB_5;
    surface.Alpha = _SampleTexture2D_52371273896446f2a10b7b546ac49482_A_7;
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