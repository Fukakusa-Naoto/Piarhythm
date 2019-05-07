Shader "UI/Texture"
{
	Properties
	{
		_MainTex("Texture",2D) = "white"{}
	}
		SubShader
	{
		Pass
	{
		Name "Frag"

		Tags
	{
		"RenderType" = "Transparent"
		"Queue" = "Transparent"
	}
		LOD 200

		CGPROGRAM
		#pragma fragment frag

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};


		float4 frag(Input IN)
		{
			return float4(1, 1, 1, 1);
			//return tex2D(_MainTex, IN.uv_MainTex);
		}
		ENDCG
	}
	}
		FallBack "Diffuse"
}
