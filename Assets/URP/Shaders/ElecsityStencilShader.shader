Shader "RulletElecsityShader"
{
    Properties
    {
        [NoScaleOffset] MainTex("MainTex", 2D) = "white" {}
        Vector1_eae739f0158341a1a3cf39fc2bf2166f("ElecsitySpeed", Float) = 1
        Vector1_231f7f0d09ab4c978db149625dd91344("ElecsityScale", Float) = 6
        Vector1_8880486bfb3d4ee099d9d06963e398d6("LineWikdth", Float) = 0.6
        [HDR]Color_5d3b4d0c608e4359a37a126d80078e68("ColorTint", Color) = (2.473216, 0, 2.619777, 0)
        Vector2_2c28b108e8b84af8b73226a3076ea704("PocalPosition", Vector) = (0.49, 0.5, 0, 0)
        Vector1_9f26ac84280b43b294bc8ed663ba58b5("PixelizeValue", Range(0, 100)) = 24.64
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
float4 MainTex_TexelSize;
float Vector1_eae739f0158341a1a3cf39fc2bf2166f;
float Vector1_231f7f0d09ab4c978db149625dd91344;
float Vector1_8880486bfb3d4ee099d9d06963e398d6;
float4 Color_5d3b4d0c608e4359a37a126d80078e68;
float2 Vector2_2c28b108e8b84af8b73226a3076ea704;
float Vector1_9f26ac84280b43b294bc8ed663ba58b5;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(MainTex);
SAMPLER(samplerMainTex);

// Graph Functions

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Floor_float4(float4 In, out float4 Out)
{
    Out = floor(In);
}

void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
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

void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Clamp_float(float In, float Min, float Max, out float Out)
{
    Out = clamp(In, Min, Max);
}

void Unity_Rectangle_float(float2 UV, float Width, float Height, out float Out)
{
    float2 d = abs(UV * 2 - 1) - float2(Width, Height);
    d = 1 - d / fwidth(d);
    Out = saturate(min(d.x, d.y));
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
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float4 _UV_dec07f9992bb4db2b3119ff7a1bfd611_Out_0 = IN.uv0;
    float2 _Property_6f6cbcc64bf44e12aeee888b6a1d8b3b_Out_0 = Vector2_2c28b108e8b84af8b73226a3076ea704;
    float2 _Subtract_abff6ef5411645c58265c80ab0577752_Out_2;
    Unity_Subtract_float2((_UV_dec07f9992bb4db2b3119ff7a1bfd611_Out_0.xy), _Property_6f6cbcc64bf44e12aeee888b6a1d8b3b_Out_0, _Subtract_abff6ef5411645c58265c80ab0577752_Out_2);
    float2 _Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2;
    Unity_Multiply_float(float2(0.5, 0.5), _Subtract_abff6ef5411645c58265c80ab0577752_Out_2, _Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2);
    float _Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1;
    Unity_Length_float2(_Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2, _Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1);
    UnityTexture2D _Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0 = UnityBuildTexture2DStructNoScale(MainTex);
    float4 _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0.tex, _Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_R_4 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.r;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_G_5 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.g;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_B_6 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.b;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_A_7 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.a;
    float4 _UV_c6a7def38c9a4ce8ac108a60176ab9f6_Out_0 = IN.uv0;
    float _Property_f317a24fcd9c4d5cbeb526caace29733_Out_0 = Vector1_9f26ac84280b43b294bc8ed663ba58b5;
    float4 _Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2;
    Unity_Multiply_float(_UV_c6a7def38c9a4ce8ac108a60176ab9f6_Out_0, (_Property_f317a24fcd9c4d5cbeb526caace29733_Out_0.xxxx), _Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2);
    float4 _Floor_5a59a385b29846eb8855eea7a9236266_Out_1;
    Unity_Floor_float4(_Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2, _Floor_5a59a385b29846eb8855eea7a9236266_Out_1);
    float4 _Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2;
    Unity_Divide_float4(_Floor_5a59a385b29846eb8855eea7a9236266_Out_1, (_Property_f317a24fcd9c4d5cbeb526caace29733_Out_0.xxxx), _Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2);
    float _Property_84c60dd6f75a4e97888a50f0180fad63_Out_0 = Vector1_eae739f0158341a1a3cf39fc2bf2166f;
    float _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2;
    Unity_Multiply_float(_Property_84c60dd6f75a4e97888a50f0180fad63_Out_0, IN.TimeParameters.x, _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2);
    float _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1;
    Unity_OneMinus_float(_Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2, _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1);
    float2 _Vector2_21c7a1df20d7478ca68fbb1f770e7cdb_Out_0 = float2(0, _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1);
    float2 _TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3;
    Unity_TilingAndOffset_float((_Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2.xy), float2 (1, 1), _Vector2_21c7a1df20d7478ca68fbb1f770e7cdb_Out_0, _TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3);
    float _Property_fac7498b683b405db48fcadc88826109_Out_0 = Vector1_231f7f0d09ab4c978db149625dd91344;
    float _GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3, _Property_fac7498b683b405db48fcadc88826109_Out_0, _GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2);
    float2 _Vector2_042f16490ad14f319d37e0e78cd79409_Out_0 = float2(0, _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2);
    float2 _TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3;
    Unity_TilingAndOffset_float((_Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2.xy), float2 (1, 1), _Vector2_042f16490ad14f319d37e0e78cd79409_Out_0, _TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3);
    float _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3, _Property_fac7498b683b405db48fcadc88826109_Out_0, _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2);
    float _Multiply_3d06441f391e4d5abd980896a91264fb_Out_2;
    Unity_Multiply_float(_GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2, _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2, _Multiply_3d06441f391e4d5abd980896a91264fb_Out_2);
    float _Remap_0e6e654d182144dbaac0f72cae97647f_Out_3;
    Unity_Remap_float(_Multiply_3d06441f391e4d5abd980896a91264fb_Out_2, float2 (0, 1), float2 (-1, 4), _Remap_0e6e654d182144dbaac0f72cae97647f_Out_3);
    float _Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3;
    Unity_Clamp_float(_Remap_0e6e654d182144dbaac0f72cae97647f_Out_3, 0, 0.8, _Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3);
    float _Property_475e793a6f8541a598b66e0b70f659f5_Out_0 = Vector1_8880486bfb3d4ee099d9d06963e398d6;
    float _Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3;
    Unity_Rectangle_float((_Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3.xx), _Property_475e793a6f8541a598b66e0b70f659f5_Out_0, _Property_475e793a6f8541a598b66e0b70f659f5_Out_0, _Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3);
    float4 _Property_c0d0f760266e4787aa484150c198efc0_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_5d3b4d0c608e4359a37a126d80078e68) : Color_5d3b4d0c608e4359a37a126d80078e68;
    float4 _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2;
    Unity_Multiply_float((_Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3.xxxx), _Property_c0d0f760266e4787aa484150c198efc0_Out_0, _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2);
    float4 _Add_ecb441433e054f328d0feb12ac023a86_Out_2;
    Unity_Add_float4(_SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0, _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2, _Add_ecb441433e054f328d0feb12ac023a86_Out_2);
    float4 _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2;
    Unity_Multiply_float((_Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1.xxxx), _Add_ecb441433e054f328d0feb12ac023a86_Out_2, _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2);
    float _Split_b098d774946a43abacd73fd837fc6990_R_1 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[0];
    float _Split_b098d774946a43abacd73fd837fc6990_G_2 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[1];
    float _Split_b098d774946a43abacd73fd837fc6990_B_3 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[2];
    float _Split_b098d774946a43abacd73fd837fc6990_A_4 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[3];
    float4 _Combine_e434593e90d149639f87aa1875840a83_RGBA_4;
    float3 _Combine_e434593e90d149639f87aa1875840a83_RGB_5;
    float2 _Combine_e434593e90d149639f87aa1875840a83_RG_6;
    Unity_Combine_float(_Split_b098d774946a43abacd73fd837fc6990_R_1, _Split_b098d774946a43abacd73fd837fc6990_G_2, _Split_b098d774946a43abacd73fd837fc6990_B_3, 0, _Combine_e434593e90d149639f87aa1875840a83_RGBA_4, _Combine_e434593e90d149639f87aa1875840a83_RGB_5, _Combine_e434593e90d149639f87aa1875840a83_RG_6);
    surface.BaseColor = _Combine_e434593e90d149639f87aa1875840a83_RGB_5;
    surface.Alpha = _Split_b098d774946a43abacd73fd837fc6990_A_4;
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
float4 MainTex_TexelSize;
float Vector1_eae739f0158341a1a3cf39fc2bf2166f;
float Vector1_231f7f0d09ab4c978db149625dd91344;
float Vector1_8880486bfb3d4ee099d9d06963e398d6;
float4 Color_5d3b4d0c608e4359a37a126d80078e68;
float2 Vector2_2c28b108e8b84af8b73226a3076ea704;
float Vector1_9f26ac84280b43b294bc8ed663ba58b5;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(MainTex);
SAMPLER(samplerMainTex);

// Graph Functions

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Floor_float4(float4 In, out float4 Out)
{
    Out = floor(In);
}

void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
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

void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Clamp_float(float In, float Min, float Max, out float Out)
{
    Out = clamp(In, Min, Max);
}

void Unity_Rectangle_float(float2 UV, float Width, float Height, out float Out)
{
    float2 d = abs(UV * 2 - 1) - float2(Width, Height);
    d = 1 - d / fwidth(d);
    Out = saturate(min(d.x, d.y));
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
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float4 _UV_dec07f9992bb4db2b3119ff7a1bfd611_Out_0 = IN.uv0;
    float2 _Property_6f6cbcc64bf44e12aeee888b6a1d8b3b_Out_0 = Vector2_2c28b108e8b84af8b73226a3076ea704;
    float2 _Subtract_abff6ef5411645c58265c80ab0577752_Out_2;
    Unity_Subtract_float2((_UV_dec07f9992bb4db2b3119ff7a1bfd611_Out_0.xy), _Property_6f6cbcc64bf44e12aeee888b6a1d8b3b_Out_0, _Subtract_abff6ef5411645c58265c80ab0577752_Out_2);
    float2 _Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2;
    Unity_Multiply_float(float2(0.5, 0.5), _Subtract_abff6ef5411645c58265c80ab0577752_Out_2, _Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2);
    float _Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1;
    Unity_Length_float2(_Multiply_1caa9e16cfdd4e77bb5ba4349bbc88cb_Out_2, _Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1);
    UnityTexture2D _Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0 = UnityBuildTexture2DStructNoScale(MainTex);
    float4 _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0.tex, _Property_c5f13d3f514d47608e1a032fc1eba23a_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_R_4 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.r;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_G_5 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.g;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_B_6 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.b;
    float _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_A_7 = _SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0.a;
    float4 _UV_c6a7def38c9a4ce8ac108a60176ab9f6_Out_0 = IN.uv0;
    float _Property_f317a24fcd9c4d5cbeb526caace29733_Out_0 = Vector1_9f26ac84280b43b294bc8ed663ba58b5;
    float4 _Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2;
    Unity_Multiply_float(_UV_c6a7def38c9a4ce8ac108a60176ab9f6_Out_0, (_Property_f317a24fcd9c4d5cbeb526caace29733_Out_0.xxxx), _Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2);
    float4 _Floor_5a59a385b29846eb8855eea7a9236266_Out_1;
    Unity_Floor_float4(_Multiply_8d26c46e783d49b3a28c1ebecf6d80c0_Out_2, _Floor_5a59a385b29846eb8855eea7a9236266_Out_1);
    float4 _Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2;
    Unity_Divide_float4(_Floor_5a59a385b29846eb8855eea7a9236266_Out_1, (_Property_f317a24fcd9c4d5cbeb526caace29733_Out_0.xxxx), _Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2);
    float _Property_84c60dd6f75a4e97888a50f0180fad63_Out_0 = Vector1_eae739f0158341a1a3cf39fc2bf2166f;
    float _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2;
    Unity_Multiply_float(_Property_84c60dd6f75a4e97888a50f0180fad63_Out_0, IN.TimeParameters.x, _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2);
    float _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1;
    Unity_OneMinus_float(_Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2, _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1);
    float2 _Vector2_21c7a1df20d7478ca68fbb1f770e7cdb_Out_0 = float2(0, _OneMinus_201ee86a842f4240a592f1fdb2b1eefa_Out_1);
    float2 _TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3;
    Unity_TilingAndOffset_float((_Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2.xy), float2 (1, 1), _Vector2_21c7a1df20d7478ca68fbb1f770e7cdb_Out_0, _TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3);
    float _Property_fac7498b683b405db48fcadc88826109_Out_0 = Vector1_231f7f0d09ab4c978db149625dd91344;
    float _GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_f7c2c4734016453eab8c5ec0f34654bb_Out_3, _Property_fac7498b683b405db48fcadc88826109_Out_0, _GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2);
    float2 _Vector2_042f16490ad14f319d37e0e78cd79409_Out_0 = float2(0, _Multiply_7f5937c6a4f34083a2cb1e8e2fd9fc54_Out_2);
    float2 _TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3;
    Unity_TilingAndOffset_float((_Divide_9141714b87654fc7bb57706c24e8a7c0_Out_2.xy), float2 (1, 1), _Vector2_042f16490ad14f319d37e0e78cd79409_Out_0, _TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3);
    float _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_0d5569156ec147309dfddc52237198a0_Out_3, _Property_fac7498b683b405db48fcadc88826109_Out_0, _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2);
    float _Multiply_3d06441f391e4d5abd980896a91264fb_Out_2;
    Unity_Multiply_float(_GradientNoise_1d6dc85648fa4f529cd8e8a1689d24ad_Out_2, _GradientNoise_f676b387faa94e3ea7e10b9c76a56c29_Out_2, _Multiply_3d06441f391e4d5abd980896a91264fb_Out_2);
    float _Remap_0e6e654d182144dbaac0f72cae97647f_Out_3;
    Unity_Remap_float(_Multiply_3d06441f391e4d5abd980896a91264fb_Out_2, float2 (0, 1), float2 (-1, 4), _Remap_0e6e654d182144dbaac0f72cae97647f_Out_3);
    float _Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3;
    Unity_Clamp_float(_Remap_0e6e654d182144dbaac0f72cae97647f_Out_3, 0, 0.8, _Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3);
    float _Property_475e793a6f8541a598b66e0b70f659f5_Out_0 = Vector1_8880486bfb3d4ee099d9d06963e398d6;
    float _Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3;
    Unity_Rectangle_float((_Clamp_e8cbdf70ecf844b291f1717b139a43aa_Out_3.xx), _Property_475e793a6f8541a598b66e0b70f659f5_Out_0, _Property_475e793a6f8541a598b66e0b70f659f5_Out_0, _Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3);
    float4 _Property_c0d0f760266e4787aa484150c198efc0_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_5d3b4d0c608e4359a37a126d80078e68) : Color_5d3b4d0c608e4359a37a126d80078e68;
    float4 _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2;
    Unity_Multiply_float((_Rectangle_d4bab5751f01418287e456cdfe65323e_Out_3.xxxx), _Property_c0d0f760266e4787aa484150c198efc0_Out_0, _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2);
    float4 _Add_ecb441433e054f328d0feb12ac023a86_Out_2;
    Unity_Add_float4(_SampleTexture2D_8bbe09b7f596424392d5f2a6a828b399_RGBA_0, _Multiply_3ab0c7b059cb44a2a0efdd7ed9fb676f_Out_2, _Add_ecb441433e054f328d0feb12ac023a86_Out_2);
    float4 _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2;
    Unity_Multiply_float((_Length_3a2ab8031f63456ab60061a8d3dd3570_Out_1.xxxx), _Add_ecb441433e054f328d0feb12ac023a86_Out_2, _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2);
    float _Split_b098d774946a43abacd73fd837fc6990_R_1 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[0];
    float _Split_b098d774946a43abacd73fd837fc6990_G_2 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[1];
    float _Split_b098d774946a43abacd73fd837fc6990_B_3 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[2];
    float _Split_b098d774946a43abacd73fd837fc6990_A_4 = _Multiply_6fc94a0566a74067802db72cd6995c60_Out_2[3];
    float4 _Combine_e434593e90d149639f87aa1875840a83_RGBA_4;
    float3 _Combine_e434593e90d149639f87aa1875840a83_RGB_5;
    float2 _Combine_e434593e90d149639f87aa1875840a83_RG_6;
    Unity_Combine_float(_Split_b098d774946a43abacd73fd837fc6990_R_1, _Split_b098d774946a43abacd73fd837fc6990_G_2, _Split_b098d774946a43abacd73fd837fc6990_B_3, 0, _Combine_e434593e90d149639f87aa1875840a83_RGBA_4, _Combine_e434593e90d149639f87aa1875840a83_RGB_5, _Combine_e434593e90d149639f87aa1875840a83_RG_6);
    surface.BaseColor = _Combine_e434593e90d149639f87aa1875840a83_RGB_5;
    surface.Alpha = _Split_b098d774946a43abacd73fd837fc6990_A_4;
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
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}