Shader "Unlit/AirSphereShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		
		_Color("Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Float) = 1
		_Speed("Speed", Float) = 1
		_IsOppMap("IsOppMap", Float) = 0
		_IsYDirBool("IsYDirBool", Float) = 0
		_IsYDirCutoff("IsYDirCutoff", Float) = 0

		_StartCutoff("Start Cutoff", Range(0, 1)) = 0
		_EndCutoff("End Cutoff", Range(0, 1)) = 1
		_StartCutoffRange("Start Cutoff Range", Range(0, 1)) = 0.1
		_EndCutoffRange("End Cutoff Range", Range(0, 1)) = 0.1

			_DissolveTex("Dissolve Texture", 2D) = "white" {}
			_DissolveProgress("Dissolve Progress", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100

		Blend OneMinusDstColor One
		Cull Off

        Pass
        {
			Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _Color;
			float _Brightness;
			float _Speed;
			float _IsOppMap;
			float _IsYDirBool;
			float _IsYDirCutoff;
			float _StartCutoff;
			float _EndCutoff;
			float _StartCutoffRange;
			float _EndCutoffRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			sampler2D _DissolveTex;
			float _DissolveProgress;

            fixed4 frag (v2f i) : SV_Target
            {
				float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
				clip(dissolvevalue);

				float2 Offset = float2(0, 0);
				if (_IsYDirBool >= 1) Offset = float2(0, 1) * _Speed * _Time.x;
				else Offset = float2(1, 0) * _Speed * _Time.x;

				float2 UV = float2(0, 0);
				if (_IsOppMap >= 1) UV = i.uv.yx;
				else UV = i.uv.xy;

				UV.x /= 1.5;

				//New
				UV.y += UV.x * 3;

                fixed4 col = tex2D(_MainTex, UV + Offset);

				

				col *= _Brightness;

				//col *= smoothstep(0, 0.35, i.uv.y);

				float UVToCompare = i.uv.x;
				if (_IsYDirCutoff >= 1) UVToCompare = i.uv.y;

				col *= smoothstep(_StartCutoff - _StartCutoffRange, _StartCutoff, UVToCompare);
				col *= 1 - smoothstep(_EndCutoff, _EndCutoff + _EndCutoffRange, UVToCompare);

				if (col.a <= 0) clip(-1);

				col *= _Color;

                return col;
            }
            ENDCG
        }
		
		Pass
		{
				Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				float4 _Color;
				float _Brightness;
				float _Speed;
				float _IsOppMap;
				float _IsYDirBool;
				float _IsYDirCutoff;
				float _StartCutoff;
				float _EndCutoff;
				float _StartCutoffRange;
				float _EndCutoffRange;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				sampler2D _DissolveTex;
				float _DissolveProgress;

				fixed4 frag(v2f i) : SV_Target
				{
				float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
				clip(dissolvevalue);

					float2 Offset = float2(0, 0);
					if (_IsYDirBool >= 1) Offset = float2(0, 1) * _Speed * _Time.x;
					else Offset = float2(1, 0) * _Speed * _Time.x;

					float2 UV = float2(0, 0);
					if (_IsOppMap >= 1) UV = i.uv.yx;
					else UV = i.uv.xy;

					UV.x /= 1.5;
					
					//New
					UV.y += UV.x * 3;

					fixed4 col = tex2D(_MainTex, UV + Offset);

					//col += _Color;

					col *= _Brightness;

					//col *= smoothstep(0, 0.35, i.uv.y);

					float UVToCompare = i.uv.x;
					if (_IsYDirCutoff >= 1) UVToCompare = i.uv.y;

					col *= smoothstep(_StartCutoff - _StartCutoffRange, _StartCutoff, UVToCompare);
					col *= 1 - smoothstep(_EndCutoff, _EndCutoff + _EndCutoffRange, UVToCompare);

					if (col.a <= 0) clip(-1);

					col *= _Color;

					return col;
				}
				ENDCG
		}
    }
}
