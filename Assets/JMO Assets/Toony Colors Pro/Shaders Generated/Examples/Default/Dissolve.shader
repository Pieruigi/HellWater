﻿// Toony Colors Pro+Mobile 2
// (c) 2014-2020 Jean Moreno

Shader "Toony Colors Pro 2/Examples/Default/Dissolve"
{
	Properties
	{
	[TCP2HeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}
	[TCP2Separator]

		//TOONY COLORS RAMP
		[TCP2Header(RAMP SETTINGS)]

		_RampThreshold ("Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.001,1)) = 0.1
	[TCP2Separator]

	[TCP2HeaderHelp(TRANSPARENCY)]
		//Blending
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendTCP2 ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendTCP2 ("Blending Dest", Float) = 10
		//Alpha Testing
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	[TCP2Separator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend [_SrcBlendTCP2] [_DstBlendTCP2]

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom fullforwardshadows keepalpha exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		fixed _SketchSpeed;
		fixed _Cutoff;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		half _RampThreshold;
		half _RampSmooth;

		// Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};

		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
		{
		#define IN_NORMAL s.Normal
	
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			IN_NORMAL = normalize(IN_NORMAL);
			fixed ndl = max(0, dot(IN_NORMAL, lightDir));
			#define NDL ndl

			#define		RAMP_THRESHOLD	_RampThreshold
			#define		RAMP_SMOOTH		_RampSmooth

			fixed3 ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, NDL);
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
		// Note: we consider that a directional light with a cookie is supposed to be the main one (even though Unity renders it as an additional light).
		// Thus when using a main directional light AND another directional light with a cookie, then the shadow color might be applied twice.
		// You can remove the DIRECTIONAL_COOKIE check below the prevent that.
		#if !defined(UNITY_PASS_FORWARDBASE) && !defined(DIRECTIONAL_COOKIE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
			c.a = s.Alpha;

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif

		#if defined(UNITY_PASS_FORWARDADD)
			//multiply RGB with alpha for additive lights for proper transparency behavior
			c.rgb *= c.a;
		#endif

			return c;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
		{
			gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);

			s.atten = data.atten;	//transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
	
			//Cutout (Alpha Testing)
			clip (o.Alpha - _Cutoff);
		}

		ENDCG

		//Shadow Caster (for shadows and depth texture)
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing


			half4		_Color;
			half		_Cutoff;
			sampler2D	_MainTex;
			float4		_MainTex_ST;
			sampler3D	_DitherMaskLOD;

			struct VertexInput
			{
				float4 vertex	: POSITION;
				float3 normal	: NORMAL;
				float2 uv0		: TEXCOORD0;
		#if UNITY_VERSION >= 550
				UNITY_VERTEX_INPUT_INSTANCE_ID
		#endif
			};

			struct VertexOutputShadowCaster
			{
				V2F_SHADOW_CASTER_NOPOS
				float2 tex : TEXCOORD1;
			};

			void vertShadowCaster(VertexInput v, out VertexOutputShadowCaster o, out float4 opos : SV_POSITION)
			{
				
				TRANSFER_SHADOW_CASTER_NOPOS(o,opos)
				o.tex = TRANSFORM_TEX(v.uv0, _MainTex);
			}

			half4 fragShadowCaster(VertexOutputShadowCaster i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
			{
				half alpha = tex2D(_MainTex, i.tex).a * _Color.a;
				// Use dither mask for alpha blended shadows, based on pixel position xy
				// and alpha level. Our dither texture is 4x4x16.
				half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
				clip (alphaRef - 0.01);

				SHADOW_CASTER_FRAGMENT(i)
			}

			ENDCG
		}
	}

	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector_SG"
}
