float2 canvasSize;
float2 textureSize;
float scale;
float multiplier;

sampler baseSampler : register(s0);
sampler textureSampler : register(s1) = sampler_state
{
	AddressU = Wrap;
	AddressV = Wrap;
};

// Scale texture coordinates based on canvas size and texture size
float2 resizeTexCoords(float2 texCoords)
{
	return texCoords * (canvasSize / textureSize);
}

// Opaque
float4 PSOpaque(float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 final = tex2D(textureSampler, resizeTexCoords(texCoords) / scale);
	final.rgb *= multiplier;
	return final;
}

// Overlay
float4 PSOverlay(float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = tex2D(textureSampler, resizeTexCoords(texCoords) / scale);
	
	base.rgb =  lerp(base.rgb, base.rgb * tex.rgb, tex.a) * multiplier;
	
	return base;
}

// Additive
float4 PSAdditive(float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = tex2D(textureSampler, resizeTexCoords(texCoords) / scale);
	
	base.rgb += tex.rgb * multiplier;
	
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
