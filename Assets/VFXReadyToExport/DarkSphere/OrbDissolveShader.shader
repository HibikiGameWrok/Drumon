Shader "Unlit/OrbDissolveShader"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1)

		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveProgress("Dissolve Progress", Range(0, 1)) = 0

		_DissolveColor("Dissolve Color", Color) = (1, 0.5, 0.5, 1)

						_IriTex("IriTexture", 2D) = "white" {}
			_IriMag("IriMagnification", Float) = 1

			_SpeedX("Speed X", Float) = 1
			_SpeedY("Speed Y", Float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100

		Cull Off

        Pass
        {
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
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

				float3 worldNormal : NORMAL;

				float3 viewDir : TEXCOORD4;

				float4 worldPos : TEXCOORD5;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _DissolveColor;

			sampler2D _IriTex;
			float4 _IriTex_ST;

			sampler2D _DissolveTex;
			float _DissolveProgress;

			float _SpeedX;
			float _SpeedY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float _IriMag;
			
            fixed4 frag (v2f i) : SV_Target
            {
                //// sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);

					float dissolvevalue = tex2D(_DissolveTex, i.uv + float2(_SpeedX, _SpeedY) * _Time.x).r - _DissolveProgress;
					clip(dissolvevalue);
					float LineTransitionAmount = 0;

					//LineTransitionAmount = 1 - smoothstep(0.03, 0.08, dissolvevalue);

					float  NDotV = dot(normalize(i.worldNormal), normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz));
					float2 NDotV2 = float2(NDotV, NDotV);
					float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));

					fixed4 col = _Color;
					//col *= lerp(_Color, _DissolveColor, LineTransitionAmount);

					col *= IriColor * _IriMag;
					col += _Color * 0.5;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
