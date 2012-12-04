float2 canvasSize;
float scale;
float multiplier;

sampler baseSampler : register(s0);
sampler textureSampler : register(s1) = sampler_state
{
	AddressU = Wrap;
	AddressV = Wrap;
};

// Opaque
float4 PSOpaque(float2 texCoords : TEXCOORD0) : COLOR0
{
	return tex2D(textureSampler, texCoords);
}

// Overlay
float4 PSOverlay(float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = tex2D(textureSampler, texCoords);
	
	
	base.rgb =  lerp(base.rgb, base.rgb * tex.rgb, tex.a);
	
	return base;
}

// Additive
float4 PSAdditive(float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = tex2D(textureSampler, texCoords);
	
	base.rgb += tex.rgb;
	
	return base;
}

technique opaque
{
    pass main
    {
        PixelShader = compile ps_2_0 PSOpaque();
    }
}

technique overlay
{
	pass main
	{
		PixelShader = compile ps_2_0 PSOverlay();
	}
}

technique additive
{	
	pass main
	{
		PixelShader = compile ps_2_0 PSAdditive();
	}
}
