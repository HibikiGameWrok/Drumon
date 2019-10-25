Shader "Unlit/TriplanarToonTerrainShader"
{
	Properties
	{
		_TopColor("Top Color", Color) = (1, 1, 1, 1)
		_BottomColor("Bottom Color", Color) = (1, 1, 1, 1)

		_TopTex("Top Texture", 2D) = "white" {}
		_BottomTex("Bottom Texture", 2D) = "white" {}

		_MinDot("Minimum Dot Product", Range(-1,1)) = 0
		_DotLerp("Dot Lerp", Range(0,1)) = 0

		//Toon shader stuff below
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			#pragma multi_compile_fwdbase

				#include "UnityCG.cginc"
				//#include "UnityLightingCommon.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION; //it has to be named 'pos' for shadows to work, apparently
					float4 worldPos : TEXCOORD1;

					float dotpro : TEXCOORD2;
					float3 normal : TEXCOORD3;

					float3 worldNormal : NORMAL;
					float3 viewDir : TEXCOORD4;
					SHADOW_COORDS(5)

				};

				float4 _TopColor;
				float4 _BottomColor;

				sampler2D _TopTex;
				sampler2D _BottomTex;
				float4 _MainTex_ST;

				float _MinDot;
				float _DotLerp;

				float4 _AmbientColor;

				float _Glossiness;
				float4 _SpecularColor;

				float4 _RimColor;
				float _RimAmount;
				float _RimThreshold;

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);

					o.normal = mul(unity_ObjectToWorld, v.normal);


					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.viewDir = WorldSpaceViewDir(v.vertex);

					o.dotpro = dot(o.worldNormal, float3(0, 1, 0));

					TRANSFER_SHADOW(o)

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col, colGrass;

				// Blending factor of triplanar mapping
				float3 bf = normalize(abs(i.worldNormal));
				bf /= dot(bf, (float3)1);

				half4 cx = tex2D(_BottomTex, i.worldPos.yz) * bf.x;
				half4 cy = tex2D(_BottomTex, i.worldPos.zx) * bf.y;
				half4 cz = tex2D(_BottomTex, i.worldPos.xy) * bf.z;

				col = (cx + cy + cz) * _BottomColor;

				//cx = tex2D(_TopTex, i.worldPos.yz) * bf.x;
				//cy = tex2D(_TopTex, i.worldPos.zx) * bf.y;
				//cz = tex2D(_TopTex, i.worldPos.xy) * bf.z;

				//colGrass = (cx + cy + cz) * _TopColor;

				//col = lerp(col, colGrass, (i.dotpro - (_MinDot - _DotLerp / 2)) / _DotLerp);

				cx = tex2D(_TopTex, i.worldPos.yz) * bf.x;
				cy = tex2D(_TopTex, i.worldPos.zx) * bf.y;
				cz = tex2D(_TopTex, i.worldPos.xy) * bf.z;

				colGrass = (cx + cy + cz) * _TopColor;

				if (_MinDot + _DotLerp / 2 >= i.dotpro && i.dotpro >= _MinDot - _DotLerp / 2)
				{
					col = lerp(col, colGrass, (i.dotpro - (_MinDot - _DotLerp / 2)) / _DotLerp);
				}
				else if (i.dotpro >= _MinDot)
				{
					col = (cx + cy + cz) * _TopColor;
				}

				//Toon shader stuff below

				float3 normal = normalize(i.worldNormal);

				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float shadow = SHADOW_ATTENUATION(i);

				//float lightIntensity = NdotL > 0 ? 1 : 0;
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				//float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);//* NdotL;
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

					return col * (_AmbientColor + light + specular + rim);
				}

				ENDCG
			}

			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

		}

		Fallback "Diffuse"
}
