Shader "Unlit/SparkleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SparkleTex("Sparkle Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
				float4 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

				float4 worldPos : TEXCOORD2;

				float dotpro : TEXCOORD3;
				float3 normal : TEXCOORD4;

				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD5;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _SparkleTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.normal = mul(unity_ObjectToWorld, v.normal);


				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float3 bf = normalize(abs(i.worldNormal));
				bf /= dot(bf, (float3)1);

				half4 cx = tex2D(_MainTex, i.worldPos.yz) * bf.x;
				half4 cy = tex2D(_MainTex, i.worldPos.zx) * bf.y;
				half4 cz = tex2D(_MainTex, i.worldPos.xy) * bf.z;

				fixed4 col = (cx + cy + cz);

				float3 FullViewDir = i.worldPos.xyz - _WorldSpaceCameraPos.xyz;

				float3 FullVar = i.worldPos.xyz * 5 + i.viewDir.xyz * 5;

				float magni = 20;
				cx = tex2D(_SparkleTex, (FullVar.yz) / magni) * bf.x;
				cy = tex2D(_SparkleTex, (FullVar.zx) / magni) * bf.y;
				cz = tex2D(_SparkleTex, (FullVar.xy) / magni) * bf.z;

				if (floor(length(i.worldPos.xyz + i.worldPos.xyz + FullViewDir.xyz) * 5) % 2 == 0)
				col += (cx + cy + cz);

                //// apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
