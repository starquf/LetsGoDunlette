Shader "RulletWaterShader"
{
    Properties
    {
        [NoScaleOffset] _NormalTex("NormalTex", 2D) = "white" {}
        Vector2_de119d2fb17a4aa98948b0f68f98e925("PositionOffset", Vector) = (0.05, -0.05, 0, 0)
        Vector2_47cdc3f1111d478b850a2faa8dfe283d("NormalDirection", Vector) = (-0.1, 0.1, 0, 0)
        Vector1_ac125a03065d45ea825275174371e270("NormalMapMoveSpeed", Float) = 1
        Vector2_26891710b908471c9595ba6d4c024ace("NoiseDir", Vector) = (-0.5, 0.5, 0, 0)
        [HDR]Color_1d6caaa9227e48b7aa0b9cea86f1fed3("NoiseColor", Color) = (0.5209134, 2.015213, 2.007389, 0)
        Vector1_18fbdcd160d0493b8d24210610ca41b8("WaterSpeed", Float) = 0.2
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        [HDR]Color_d45af51239fd4f65944fe1d574db43f3("OriginColor", Color) = (0, 0.5529412, 1, 0)
        [NoScaleOffset]Texture2D_5c228060e83847fbb73d543cb6c464d2("AlphaMap", 2D) = "white" {}
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
        float3 TimeParameters;
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
float4 Color_d45af51239fd4f65944fe1d574db43f3;
float4 Texture2D_5c228060e83847fbb73d543cb6c464d2_TexelSize;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_NormalTex);
SAMPLER(sampler_NormalTex);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(Texture2D_5c228060e83847fbb73d543cb6c464d2);
SAMPLER(samplerTexture2D_5c228060e83847fbb73d543cb6c464d2);

// Graph Functions

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
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

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
{
    Out = A * B;
}

void Unity_Blend_Lighten_float3(float3 Base, float3 Blend, out float3 Out, float Opacity)
{
    Out = max(Blend, Base);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Blend_SoftLight_float3(float3 Base, float3 Blend, out float3 Out, float Opacity)
{
    float3 result1 = 2.0 * Base * Blend + Base * Base * (1.0 - 2.0 * Blend);
    float3 result2 = sqrt(Base) * (2.0 * Blend - 1.0) + 2.0 * Base * (1.0 - Blend);
    float3 zeroOrOne = step(0.5, Blend);
    Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
    Out = lerp(Base, Out, Opacity);
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
    float4 _Property_dfc558b1675a4d93848aac67c91932f9_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_1d6caaa9227e48b7aa0b9cea86f1fed3) : Color_1d6caaa9227e48b7aa0b9cea86f1fed3;
    float4 _Property_89f0bef966da47329e40eff285683a20_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_d45af51239fd4f65944fe1d574db43f3) : Color_d45af51239fd4f65944fe1d574db43f3;
    float _Property_01e11a09bc3d480882295cd7bca6ac17_Out_0 = Vector1_18fbdcd160d0493b8d24210610ca41b8;
    float2 _Property_375e1810f029424092430f4e3119e70e_Out_0 = Vector2_26891710b908471c9595ba6d4c024ace;
    float2 _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2;
    Unity_Multiply_float(_Property_375e1810f029424092430f4e3119e70e_Out_0, (IN.TimeParameters.x.xx), _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2);
    float2 _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2;
    Unity_Multiply_float((_Property_01e11a09bc3d480882295cd7bca6ac17_Out_0.xx), _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2, _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2);
    float2 _TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2, _TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3);
    float _GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3, 10, _GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2);
    float4 _Multiply_441446eedf034d288b69e51f19f1225a_Out_2;
    Unity_Multiply_float(_Property_89f0bef966da47329e40eff285683a20_Out_0, (_GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2.xxxx), _Multiply_441446eedf034d288b69e51f19f1225a_Out_2);
    float4 _Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2;
    Unity_Multiply_float(_Property_dfc558b1675a4d93848aac67c91932f9_Out_0, _Multiply_441446eedf034d288b69e51f19f1225a_Out_2, _Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2);
    float4 _Add_38ad3009f5044e2899261a57049b8f0f_Out_2;
    Unity_Add_float4(_Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2, float4(0, 0, 0, 0), _Add_38ad3009f5044e2899261a57049b8f0f_Out_2);
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_R_1 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[0];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_G_2 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[1];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_B_3 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[2];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_A_4 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[3];
    float4 _Combine_f73c1a1748d341b6a774cb17c30a3360_RGBA_4;
    float3 _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5;
    float2 _Combine_f73c1a1748d341b6a774cb17c30a3360_RG_6;
    Unity_Combine_float(_Split_b71b1fd1dc9f459ea6c767d9e06fa644_R_1, _Split_b71b1fd1dc9f459ea6c767d9e06fa644_G_2, _Split_b71b1fd1dc9f459ea6c767d9e06fa644_B_3, 0, _Combine_f73c1a1748d341b6a774cb17c30a3360_RGBA_4, _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5, _Combine_f73c1a1748d341b6a774cb17c30a3360_RG_6);
    float3 _Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2;
    Unity_Multiply_float((_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7.xxx), _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5, _Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2);
    float4 _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4;
    float3 _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    float2 _Combine_7e23858362e34f3fa98ce264e1208067_RG_6;
    Unity_Combine_float(_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6, 0, _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Combine_7e23858362e34f3fa98ce264e1208067_RG_6);
    float3 _Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2;
    Unity_Blend_Lighten_float3(_Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7);
    UnityTexture2D _Property_c03d25eebdc848ef87d22810b5ba6420_Out_0 = UnityBuildTexture2DStructNoScale(_NormalTex);
    float2 _Property_cc3a979fe1334d27b4057d614452a03d_Out_0 = Vector2_47cdc3f1111d478b850a2faa8dfe283d;
    float2 _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2;
    Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_cc3a979fe1334d27b4057d614452a03d_Out_0, _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2);
    float2 _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2, _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3);
    float4 _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c03d25eebdc848ef87d22810b5ba6420_Out_0.tex, _Property_c03d25eebdc848ef87d22810b5ba6420_Out_0.samplerstate, _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3);
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_R_4 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.r;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_G_5 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.g;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_B_6 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.b;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_A_7 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.a;
    float _Property_0e6b3d8587f646d19de9b0f3eed0e51d_Out_0 = Vector1_ac125a03065d45ea825275174371e270;
    float4 _Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2;
    Unity_Multiply_float(_SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0, (_Property_0e6b3d8587f646d19de9b0f3eed0e51d_Out_0.xxxx), _Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2);
    float3 _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2;
    Unity_Blend_SoftLight_float3(_Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2, (_Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2.xyz), _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2, 1);
    UnityTexture2D _Property_bb131ef5ac27435890d14196dfc04a65_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_5c228060e83847fbb73d543cb6c464d2);
    float4 _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bb131ef5ac27435890d14196dfc04a65_Out_0.tex, _Property_bb131ef5ac27435890d14196dfc04a65_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_R_4 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.r;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_G_5 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.g;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_B_6 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.b;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_A_7 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.a;
    surface.BaseColor = _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2;
    surface.Alpha = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_A_7;
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
        float3 TimeParameters;
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
float4 Color_d45af51239fd4f65944fe1d574db43f3;
float4 Texture2D_5c228060e83847fbb73d543cb6c464d2_TexelSize;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_NormalTex);
SAMPLER(sampler_NormalTex);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(Texture2D_5c228060e83847fbb73d543cb6c464d2);
SAMPLER(samplerTexture2D_5c228060e83847fbb73d543cb6c464d2);

// Graph Functions

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
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

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
{
    Out = A * B;
}

void Unity_Blend_Lighten_float3(float3 Base, float3 Blend, out float3 Out, float Opacity)
{
    Out = max(Blend, Base);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Blend_SoftLight_float3(float3 Base, float3 Blend, out float3 Out, float Opacity)
{
    float3 result1 = 2.0 * Base * Blend + Base * Base * (1.0 - 2.0 * Blend);
    float3 result2 = sqrt(Base) * (2.0 * Blend - 1.0) + 2.0 * Base * (1.0 - Blend);
    float3 zeroOrOne = step(0.5, Blend);
    Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
    Out = lerp(Base, Out, Opacity);
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
    float4 _Property_dfc558b1675a4d93848aac67c91932f9_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_1d6caaa9227e48b7aa0b9cea86f1fed3) : Color_1d6caaa9227e48b7aa0b9cea86f1fed3;
    float4 _Property_89f0bef966da47329e40eff285683a20_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_d45af51239fd4f65944fe1d574db43f3) : Color_d45af51239fd4f65944fe1d574db43f3;
    float _Property_01e11a09bc3d480882295cd7bca6ac17_Out_0 = Vector1_18fbdcd160d0493b8d24210610ca41b8;
    float2 _Property_375e1810f029424092430f4e3119e70e_Out_0 = Vector2_26891710b908471c9595ba6d4c024ace;
    float2 _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2;
    Unity_Multiply_float(_Property_375e1810f029424092430f4e3119e70e_Out_0, (IN.TimeParameters.x.xx), _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2);
    float2 _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2;
    Unity_Multiply_float((_Property_01e11a09bc3d480882295cd7bca6ac17_Out_0.xx), _Multiply_e8b8f089afdf4a55a59d21ba354315f4_Out_2, _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2);
    float2 _TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_39b97e99e5df44b9900dd68f019ec0a6_Out_2, _TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3);
    float _GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_97ebfb34eb4e478eb8360e8b8a2b2435_Out_3, 10, _GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2);
    float4 _Multiply_441446eedf034d288b69e51f19f1225a_Out_2;
    Unity_Multiply_float(_Property_89f0bef966da47329e40eff285683a20_Out_0, (_GradientNoise_6c0329af90ad435cbe0f367503417f74_Out_2.xxxx), _Multiply_441446eedf034d288b69e51f19f1225a_Out_2);
    float4 _Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2;
    Unity_Multiply_float(_Property_dfc558b1675a4d93848aac67c91932f9_Out_0, _Multiply_441446eedf034d288b69e51f19f1225a_Out_2, _Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2);
    float4 _Add_38ad3009f5044e2899261a57049b8f0f_Out_2;
    Unity_Add_float4(_Multiply_957ef43d6b2646d6a2c2fcee77cc1fb3_Out_2, float4(0, 0, 0, 0), _Add_38ad3009f5044e2899261a57049b8f0f_Out_2);
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_R_1 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[0];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_G_2 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[1];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_B_3 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[2];
    float _Split_b71b1fd1dc9f459ea6c767d9e06fa644_A_4 = _Add_38ad3009f5044e2899261a57049b8f0f_Out_2[3];
    float4 _Combine_f73c1a1748d341b6a774cb17c30a3360_RGBA_4;
    float3 _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5;
    float2 _Combine_f73c1a1748d341b6a774cb17c30a3360_RG_6;
    Unity_Combine_float(_Split_b71b1fd1dc9f459ea6c767d9e06fa644_R_1, _Split_b71b1fd1dc9f459ea6c767d9e06fa644_G_2, _Split_b71b1fd1dc9f459ea6c767d9e06fa644_B_3, 0, _Combine_f73c1a1748d341b6a774cb17c30a3360_RGBA_4, _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5, _Combine_f73c1a1748d341b6a774cb17c30a3360_RG_6);
    float3 _Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2;
    Unity_Multiply_float((_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7.xxx), _Combine_f73c1a1748d341b6a774cb17c30a3360_RGB_5, _Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2);
    float4 _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4;
    float3 _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5;
    float2 _Combine_7e23858362e34f3fa98ce264e1208067_RG_6;
    Unity_Combine_float(_SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_R_4, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_G_5, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_B_6, 0, _Combine_7e23858362e34f3fa98ce264e1208067_RGBA_4, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Combine_7e23858362e34f3fa98ce264e1208067_RG_6);
    float3 _Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2;
    Unity_Blend_Lighten_float3(_Multiply_758ffe37d5b04f7faa62942d64ecb6e5_Out_2, _Combine_7e23858362e34f3fa98ce264e1208067_RGB_5, _Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2, _SampleTexture2D_0a9bcd7c4b5547e296f59c43c45b7d82_A_7);
    UnityTexture2D _Property_c03d25eebdc848ef87d22810b5ba6420_Out_0 = UnityBuildTexture2DStructNoScale(_NormalTex);
    float2 _Property_cc3a979fe1334d27b4057d614452a03d_Out_0 = Vector2_47cdc3f1111d478b850a2faa8dfe283d;
    float2 _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2;
    Unity_Multiply_float((IN.TimeParameters.x.xx), _Property_cc3a979fe1334d27b4057d614452a03d_Out_0, _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2);
    float2 _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_fde769249a4448ccb84bba9175ea4509_Out_2, _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3);
    float4 _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c03d25eebdc848ef87d22810b5ba6420_Out_0.tex, _Property_c03d25eebdc848ef87d22810b5ba6420_Out_0.samplerstate, _TilingAndOffset_1a5d1dfa3da94e2dac500d213038a7c1_Out_3);
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_R_4 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.r;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_G_5 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.g;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_B_6 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.b;
    float _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_A_7 = _SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0.a;
    float _Property_0e6b3d8587f646d19de9b0f3eed0e51d_Out_0 = Vector1_ac125a03065d45ea825275174371e270;
    float4 _Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2;
    Unity_Multiply_float(_SampleTexture2D_ff7843513b7a466fabfbf203d1abef4d_RGBA_0, (_Property_0e6b3d8587f646d19de9b0f3eed0e51d_Out_0.xxxx), _Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2);
    float3 _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2;
    Unity_Blend_SoftLight_float3(_Blend_27cfb5e2fdb642d282d3a1ac1b5add6f_Out_2, (_Multiply_b317a1d3b38b475c97078fe74587f5ad_Out_2.xyz), _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2, 1);
    UnityTexture2D _Property_bb131ef5ac27435890d14196dfc04a65_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_5c228060e83847fbb73d543cb6c464d2);
    float4 _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bb131ef5ac27435890d14196dfc04a65_Out_0.tex, _Property_bb131ef5ac27435890d14196dfc04a65_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_R_4 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.r;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_G_5 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.g;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_B_6 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.b;
    float _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_A_7 = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_RGBA_0.a;
    surface.BaseColor = _Blend_24ec66752c8144c8968a36e02c5ad06a_Out_2;
    surface.Alpha = _SampleTexture2D_e5adac87b0ba49f2a1fed807a5d15272_A_7;
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
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}