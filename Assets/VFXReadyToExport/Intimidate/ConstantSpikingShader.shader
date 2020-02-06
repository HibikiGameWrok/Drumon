Shader "Unlit/ConstantSpikingShader"
{
    Properties
    {
		_Color("Color", Color) = (1, 1, 1, 1)

        _MainTex ("Texture", 2D) = "white" {}
		
		_NoiseTex ("Noise Texture", 2D) = "white" {}

		_DissolveTex ("Dissolve Tex", 2D) = "white" {}
		_DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0

    }
    SubShader
    {
        
        LOD 100

		Blend SrcAlpha OneMinusSrcAlpha


		Pass
		{

			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog
	#pragma shader_feature_local _ _ALPHATEST_ON 
	#define _ALPHATEST_ON

		#include "UnityCG.cginc"
	#include "Lighting.cginc"
	#include "AutoLight.cginc"

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

		sampler2D _NoiseTex;

		sampler2D _DissolveTex;
		float _DissolveProgress;

		v2f vert(appdata v)
		{
			v2f o;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);

			v.vertex.y += 0.01;

			o.vertex = UnityObjectToClipPos(v.vertex);

			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			clip(tex2D(_DissolveTex, i.uv.xy).r - _DissolveProgress);

			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv.xy);

		if (col.a <= 0.5) clip(-1);

		// apply fog
		UNITY_APPLY_FOG(i.fogCoord, col);
		return col * 0.8;
	}
	ENDCG
}
        Pass
        {

		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma shader_feature_local _ _ALPHATEST_ON 
			#define _ALPHATEST_ON

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

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

				float dist : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _Color;

			sampler2D _NoiseTex;

			sampler2D _DissolveTex;
			float _DissolveProgress;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.dist = sqrt(pow(v.vertex.x, 2) + pow(v.vertex.z, 2));

				//0 -> 0.333 -> 0.667 -> 1
				//0.5 -> 1.5 -> 0.5 -> 1.5

				float value = tex2Dlod(_NoiseTex, float4(float2(atan(v.vertex.z / v.vertex.x), 0) + float2(0, 1) * _Time.x * 2.5, 0.0, 0.0)).r;
				value = 1.0 - (cos(value / 0.1 * (44/7)) / 2 + 0.5);
				value = 0.8 + value * 0.4;
				v.vertex.xz *= value;

				o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
			clip(tex2D(_DissolveTex, i.uv.xy).r - _DissolveProgress);

                // sample the texture
                fixed4 col = lerp(float4(0, 0, 0, 1), _Color, i.dist) * tex2D(_NoiseTex, i.uv.xy);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * 0.6;
            }
            ENDCG
        }



    }
	Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}
