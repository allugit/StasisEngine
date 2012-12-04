#include <noise_functions.fx>

sampler baseSampler : register(s0) = sampler_state
{
	AddressU = Wrap;
	AddressV = Wrap;
};

float2 aspectRatio;
float2 offset;
float noiseFrequency;
float noiseGain;
float noiseLacunarity;
float multiplier;

float2 fbmOffset;
bool fbmPerlinBasis;
bool fbmCellBasis;
bool fbmInvCellBasis;
float4 noiseLowColor;
float4 noiseHighColor;
int fbmIterations;
float4x4 matrixTransform;

// scaleTexCoords
float2 scaleTexCoords(float2 texCoords)
{
	return (offset / renderSize) - (texCoords * aspectRatio) / noiseScale;
}

// getPerlin
float getPerlin(float2 texCoords)
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;
	
	// Calculate noise
	float2 coords = scaleTexCoords(texCoords) + baseValue * fbmOffset;
	return fbmPerlin(coords, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
}

// getWorley
float getWorley(float2 texCoords, bool inverse)
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;
	
	// Calculate noise
	float2 coords = scaleTexCoords(texCoords) + baseValue * fbmOffset;
	return fbmWorley(coords, inverse, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
}

// Vertex shader
void VSBase(inout float4 color:COLOR0, inout float2 texCoord:TEXCOORD0, inout float4 position:SV_Position) 
{ 
	position = mul(position, matrixTransform); 
}

// Perlin pixel shaders
float4 PSOpaquePerlin(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getPerlin(texCoords);
	value *= multiplier;
	return float4(value, value, value, 1);
}
float4 PSOverlayPerlin(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getPerlin(texCoords);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb =  lerp(base.rgb, base.rgb * tex.rgb, tex.a) * multiplier;
	
	return base;
}
float4 PSAdditivePerlin(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getPerlin(texCoords);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb += tex.rgb * multiplier;
	
	return base;
}

// Worley pixel shaders
float4 PSOpaqueWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, false);
	value *= multiplier;
	return float4(value, value, value, 1);
}
float4 PSOverlayWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, false);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb =  lerp(base.rgb, base.rgb * tex.rgb, tex.a) * multiplier;
	
	return base;
}
float4 PSAdditiveWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, false);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb += tex.rgb * multiplier;
	
	return base;
}

// Inverse Worley pixel shaders
float4 PSOpaqueInvWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, true);
	value *= multiplier;
	return float4(value, value, value, 1);
}
float4 PSOverlayInvWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, true);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb =  lerp(base.rgb, base.rgb * tex.rgb, tex.a) * multiplier;
	
	return base;
}
float4 PSAdditiveInvWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, true);
	float4 base = tex2D(baseSampler, texCoords);
	float4 tex = float4(value, value, value, 1);
	
	base.rgb += tex.rgb * multiplier;
	
	return base;
}

// Perlin techniques
technique opaque_perlin
{ 
	pass main 
	{ 
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOpaquePerlin(); 
	}
}
technique overlay_perlin
{ 
	pass main
	{ 
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOverlayPerlin();
	}
}
technique additive_perlin
{ 
	pass main 
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSAdditivePerlin();
	}
}

// Worley techniques
technique opaque_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOpaqueWorley();
	}
}
technique overlay_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOverlayWorley();
	}
}
technique additive_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSAdditiveWorley();
	}
}

// Inverse Worley techniques
technique opaque_inv_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOpaqueInvWorley();
	}
}
technique overlay_inv_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSOverlayInvWorley();
	}
}
technique additive_inv_worley
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSAdditiveInvWorley();
	}
}