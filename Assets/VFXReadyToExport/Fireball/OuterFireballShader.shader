Shader "Unlit/OuterFireballShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1) 

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

					_ShadowTier2Threshold("Shadow Tier 2 Threshold", Range(-1, 1)) = -0.3
		_ShadowTier3Threshold("Shadow Tier 3 Threshold", Range(-1, 1)) = -0.6

			_IriTex("IriTexture", 2D) = "white" {}
			_IriMag("IriMagnification", Float) = 1

		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveProgress("Dissolve Progress", Range(0, 1)) = 0

				_Strength("Strength", Range(0, 1)) = 1

			[HideInInspector] _Cutoff("", Float) = 0.5

		_RumbleTex("Rumble Tex", 2D) = "white" {}
		_RumbleMagnifier("Rumble Magnifier", Float) = 1
		_RumbleSpeed("Rumble Speed", Float) = 1

			_TipColor("Tip Color", Color) = (1, 1, 1, 1)

			[HDR]
		_EmissionColor("Color", Color) = (0,0,0)

		_EmissionMap("Emission", 2D) = "white" {}

	}
	SubShader
	{
		Tags
{
	"LightMode" = "ForwardBase"
	"PassFlags" = "OnlyDirectional"
}
		LOD 100
			//ZWrite Off
			//Cull Off

			Blend SrcAlpha OneMinusSrcAlpha

		//Pass
		//{

		//	Cull Front

		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#pragma multi_compile_fwdbase
		//	// make fog work
		//	#pragma multi_compile_fog
		//	#pragma shader_feature_local _ _ALPHATEST_ON 
		//	#include "UnityCG.cginc"
		//	#include "Lighting.cginc"
		//	#include "AutoLight.cginc"
		//	#define _ALPHATEST_ON
		//	struct appdata
		//	{
		//		float4 vertex : POSITION;
		//		float2 uv : TEXCOORD0;

		//		float3 normal : NORMAL;
		//	};

		//	struct v2f
		//	{
		//		float2 uv : TEXCOORD0;
		//		UNITY_FOG_COORDS(1)
		//		float4 pos : SV_POSITION;

		//		float3 worldNormal : NORMAL;

		//		float3 viewDir : TEXCOORD4;

		//		float4 worldPos : TEXCOORD5;

		//		SHADOW_COORDS(6)
		//	};

		//	sampler2D _MainTex;
		//	float4 _MainTex_ST;
		//	float4 _Color;
		//	
		//	float4 _AmbientColor;
		//	
		//	float _Glossiness;
		//	float4 _SpecularColor;

		//	float4 _RimColor;
		//	float _RimAmount;
		//	float _RimThreshold;
		//	
		//	float _ShadowTier2Threshold;
		//	float _ShadowTier3Threshold;

		//	sampler2D _DissolveTex;
		//	float _DissolveProgress;

		//	float _IriMag;

		//	uniform sampler2D _IriTex; uniform float4 _IriTex_ST;

		//	uniform sampler2D _RumbleTex;
		//	float _RumbleMagnifier;
		//	float _RumbleSpeed;

		//	v2f vert (appdata v)
		//	{
		//		v2f o;
		//		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		//		//UNITY_TRANSFER_FOG(o,o.vertex);

		//		o.worldNormal = UnityObjectToWorldNormal(v.normal);

		//		v.vertex.xyz += o.uv.y * _RumbleMagnifier * normalize(o.worldNormal) * tex2Dlod(_RumbleTex, float4(o.uv.xy - float2(0, _Time.x * _RumbleSpeed), 0.0, 0.0)).r;

		//		o.pos = UnityObjectToClipPos(v.vertex);

		//		o.viewDir = WorldSpaceViewDir(v.vertex);

		//		o.worldPos = mul(unity_ObjectToWorld, v.vertex);

		//		TRANSFER_SHADOW(o)

		//		return o;
		//	}
		//	
		//	float _Strength;

		//	fixed4 frag (v2f i) : SV_Target
		//	{
		//			float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
		//			clip(dissolvevalue);

		//						//// sample the texture
		//		fixed4 col = tex2D(_MainTex, i.uv);

		//		if(col.a < 0.5) discard;

		//		float3 normal = normalize(i.worldNormal);

		//		float NdotL = 1;//dot(_WorldSpaceLightPos0, normal);

		//		float shadow = SHADOW_ATTENUATION(i);

		//		float lightIntensity;
		//		float NormalValToCheck = NdotL * shadow;
		//		lightIntensity = smoothstep(0, 0.01, NormalValToCheck) * 0.8 + 0.2;

		//		float4 light = /*lightIntensity * */_LightColor0;


		//		float3 viewDir = normalize(i.viewDir);
		//		float3 halfVector = normalize(/*_WorldSpaceLightPos0 +*/ viewDir);
		//		float NdotH = dot(normal, halfVector);
		//		float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
		//		float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
		//		float4 specular = specularIntensitySmooth * _SpecularColor;

		//		float4 rimDot = 1 - dot(viewDir, normal);
		//		//float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
		//		float rimIntensity = rimDot * pow(NdotL, _RimThreshold);//* NdotL;
		//		rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
		//		float4 rim = rimIntensity * _RimColor;

		//		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
		//		float3 normalDirection = normal;
		//		float  NDotV = dot(normal, viewDir);
		//		float2 NDotV2 = float2(NDotV, NDotV);
		//		float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));

		//		return _Strength * _Color * col * (_AmbientColor + light + specular + rim + IriColor * _IriMag);
		//	}
		//	ENDCG
		//}

		Pass
		{
			Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			// make fog work
			#pragma multi_compile_fog
			#pragma shader_feature_local _ _ALPHATEST_ON 

			#pragma shader_feature _EMISSION

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#define _ALPHATEST_ON
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;

				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 pos : SV_POSITION;

				float3 worldNormal : NORMAL;

				float3 viewDir : TEXCOORD4;

				float4 worldPos : TEXCOORD5;

				SHADOW_COORDS(6)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			float4 _TipColor;
			
			float4 _AmbientColor;
			
			float _Glossiness;
			float4 _SpecularColor;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;
			
			float _ShadowTier2Threshold;
			float _ShadowTier3Threshold;

			sampler2D _DissolveTex;
			float _DissolveProgress;

			float _IriMag;

			uniform sampler2D _IriTex; uniform float4 _IriTex_ST;

			uniform sampler2D _RumbleTex;
			float _RumbleMagnifier;
			float _RumbleSpeed;

			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				v.vertex.y += 0.5;
				v.vertex.y *= 1.2;
				v.vertex.y -= 0.5;

				float valuetoburnup = pow(3, o.uv.y) - 1;

				v.vertex.xyz += valuetoburnup * _RumbleMagnifier * normalize(v.normal) * tex2Dlod(_RumbleTex, float4(o.uv.xy - float2(0, _Time.x * _RumbleSpeed), 0.0, 0.0)).r;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				TRANSFER_SHADOW(o)

				return o;
			}
			
			float _Strength;

			fixed4 frag (v2f i) : SV_Target
			{
					float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
					clip(dissolvevalue);

								//// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				if(col.a < 0.5) discard;

				float3 normal = normalize(i.worldNormal);

				float NdotL = 1;//dot(_WorldSpaceLightPos0, normal);

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity;
				float NormalValToCheck = NdotL * shadow;
				lightIntensity = smoothstep(0, 0.01, NormalValToCheck) * 0.8 + 0.2;

				float4 light = /*lightIntensity * */_LightColor0;


				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(/*_WorldSpaceLightPos0 +*/ viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				//float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);//* NdotL;
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 normalDirection = normal;
				float  NDotV = dot(normal, viewDir);
				float2 NDotV2 = float2(NDotV, NDotV);
				float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));

				_Color = lerp(_TipColor * 1.5, _Color, smoothstep(0.0, 0.8, i.uv.y));

				return _Strength * _Color * col * (_AmbientColor + light /*+ specular*/ /*+ rim*/ + IriColor * _IriMag);
			}
			ENDCG
		}

		Pass
		{
			Name "META"
			Tags { "LightMode" = "Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex vert_meta
			#pragma fragment frag_meta

			#pragma shader_feature _EMISSION
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _DETAIL_MULX2
			#pragma shader_feature EDITOR_VISUALIZATION

			
			#ifndef UNITY_STANDARD_META_INCLUDED
#define UNITY_STANDARD_META_INCLUDED

			// Functionality for Standard shader "meta" pass
			// (extracts albedo/emission for lightmapper etc.)

			#include "UnityCG.cginc"
			#include "UnityStandardInput.cginc"
			#include "UnityMetaPass.cginc"
			#include "UnityStandardCore.cginc"

			struct v2f_meta
			{
				float4 pos      : SV_POSITION;
				float4 uv       : TEXCOORD0;
			#ifdef EDITOR_VISUALIZATION
				float2 vizUV        : TEXCOORD1;
				float4 lightCoord   : TEXCOORD2;
			#endif
			};

			v2f_meta vert_meta(VertexInput v)
			{
				v2f_meta o;
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv1.xy, unity_LightmapST, unity_DynamicLightmapST);
				o.uv = TexCoords(v);
			#ifdef EDITOR_VISUALIZATION
				o.vizUV = 0;
				o.lightCoord = 0;
				if (unity_VisualizationMode == EDITORVIZ_TEXTURE)
					o.vizUV = UnityMetaVizUV(unity_EditorViz_UVIndex, v.uv0.xy, v.uv1.xy, v.uv2.xy, unity_EditorViz_Texture_ST);
				else if (unity_VisualizationMode == EDITORVIZ_SHOWLIGHTMASK)
				{
					o.vizUV = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					o.lightCoord = mul(unity_EditorViz_WorldToLight, mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)));
				}
			#endif
				return o;
			}

			// Albedo for lightmapping should basically be diffuse color.
			// But rough metals (black diffuse) still scatter quite a lot of light around, so
			// we want to take some of that into account too.
			half3 UnityLightmappingAlbedo(half3 diffuse, half3 specular, half smoothness)
			{
				half roughness = SmoothnessToRoughness(smoothness);
				half3 res = diffuse;
				res += specular * roughness * 0.5;
				return res;
			}

			float4 frag_meta(v2f_meta i) : SV_Target
			{
				// we're interested in diffuse & specular colors,
				// and surface roughness to produce final albedo.
				FragmentCommonData data = UNITY_SETUP_BRDF_INPUT(i.uv);

				UnityMetaInput o;
				UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);

			#ifdef EDITOR_VISUALIZATION
				o.Albedo = data.diffColor;
				o.VizUV = i.vizUV;
				o.LightCoord = i.lightCoord;
			#else
				o.Albedo = UnityLightmappingAlbedo(data.diffColor, data.specColor, data.smoothness);
			#endif
				o.SpecularColor = data.specColor;
				//o.Emission = Emission(i.uv.xy);
				o.Emission = _EmissionColor;

				return UnityMetaFragment(o);
			}

			#endif // UNITY_STANDARD_META_INCLUDED

			ENDCG
		}


		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"

}
