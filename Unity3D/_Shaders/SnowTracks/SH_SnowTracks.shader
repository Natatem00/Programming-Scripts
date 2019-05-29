Shader "Custom/SnowTracks"
{
    Properties
    {
		_Tess ("Tesselation", Range(1, 32)) = 4
		_SnowTex("Snow Texture", 2D) = "white" {}
        _SnowColor ("Snow Color", Color) = (1,1,1,1)
		_GroundTex("Ground Texture", 2D) = "white" {}
		_GroundColor("Ground Color", Color) = (1,1,1,1)
		_Splat ("Splat Texture", 2D) = "black" {}
		_Displacement ("Displacement", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance

        // Use shader model 4.6 target, to get nicer looking lighting
        #pragma target 4.6

		#include "Tessellation.cginc"

		struct VS_INPUT 
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		float _Tess;

		float4 tessDistance(VS_INPUT v0, VS_INPUT v1, VS_INPUT v2)
		{
			float minDis = 10;
			float maxDis = 25;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDis, maxDis, _Tess);
		}

		sampler2D _Splat;
		float _Displacement;

		void disp(inout VS_INPUT v)
		{
			float d = tex2Dlod(_Splat, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			v.vertex.xyz -= v.normal * d;
			v.vertex.xyz += v.normal * _Displacement;
		}

        sampler2D _GroundTex;
		fixed4 _GroundColor;

		sampler2D _SnowTex;
		fixed4 _SnowColor;

        struct Input
        {
            float2 uv_GroundTex;
			float2 uv_SnowTex;
			float2 uv_Splat;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half amount = tex2Dlod(_Splat, float4(IN.uv_Splat.xy, 0, 0)).r;

			fixed4 c = lerp(tex2D(_SnowTex, IN.uv_SnowTex) * _SnowColor, tex2D(_GroundTex, IN.uv_GroundTex) * _GroundColor, amount);

            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
