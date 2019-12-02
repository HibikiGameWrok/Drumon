Shader "Unlit/ProtectionSphereShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		_HitColor("Hit Color", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

			Blend SrcAlpha OneMinusSrcAlpha

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

				float4 worldPos : TEXCOORD1;

				float3 worldNormal : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				float3 normal : TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				o.normal = v.normal;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float4 _Color;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;

			float4 _HitColor;

			int _HitAmount = 0;
			float4 _HitData[20];

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;

			float3 bf = normalize(abs(i.worldNormal));
			bf /= dot(bf, (float3)1);

			float aspeed = -_Time.y;

			half4 cx = tex2D(_MainTex, i.worldPos.yz) * bf.x;
			half4 cy = tex2D(_MainTex, i.worldPos.zx) * bf.y;
			half4 cz = tex2D(_MainTex, i.worldPos.xy) * bf.z;

			float4 rimDot = 1 - dot(normalize(i.viewDir), normalize(i.worldNormal));
			float rimIntensity = rimDot * pow(1, _RimThreshold);//* NdotL;
			//rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);

			float tempval = rimIntensity - 0.3;
			//tempval += smoothstep(0.85, 1, dot(float3(0, 0, 1), normalize(i.normal))) * 0.5;

			col = (cx + cy + cz) * _Color * clamp(tempval, 0.03, 1);

			col = lerp(col * 2, _RimColor, smoothstep(0.6, 0.9, rimIntensity));

			for (int j = 0; j < _HitAmount; ++j)
			{
				float HitProgress = _HitData[j].w;
				if (_HitData[j].w > 1)
				{
					_HitData[j].w -= 1;
					_HitData[j].w = 1 - _HitData[j].w;
				}
				col += _HitData[j].w * (cx + cy + cz) *_HitColor * smoothstep(0.97, 1, dot(float3(_HitData[j].x, _HitData[j].y, _HitData[j].z), normalize(i.normal)));
			}

			//col += (cx + cy + cz) *_HitColor * smoothstep(0.85, 1, dot(float3(0, 0, 1), normalize(i.normal)));



			col *= 1 - smoothstep(0.7, 0.9, rimIntensity);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
