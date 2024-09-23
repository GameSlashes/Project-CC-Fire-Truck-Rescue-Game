Shader "Custom/AnimateArrowShader" {
	Properties {
		_MainTex ("Arrow Texture", 2D) = "white" {}
		_Color ("Main Color", Vector) = (1,1,1,1)
		_Emission ("Emission", Vector) = (0,0,0,0)
		_Cutout ("Alpha cutoff", Range(0, 1)) = 0.5
		_Speed ("Scroll Speed", Range(0, 10)) = 1
		_GlowColor ("Glow Color", Vector) = (1,1,1,1)
		_GlowPower ("Glow Power", Range(0, 10)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}