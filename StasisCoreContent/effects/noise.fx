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
float4 noiseLowColor;
float4 noiseHighColor;
float2 fbmOffset;
bool fbmPerlinBasis;
bool fbmCellBasis;
bool fbmInvCellBasis;
int fbmIterations;

int opaqueBlend = 0;
int overlayBlend = 1;
int additiveBlend = 2;
int blendType;
int worleyFeature;	// 0 = F1, 1 = F2, 2 = F2-F1 -- defined in noise_functions.fx
bool inverseWorley;
float4x4 matrixTransform;

// scaleTexCoords
float2 scaleTexCoords(float2 texCoords)
{
	return (offset / renderSize) - (texCoords * aspectRatio) / noiseScale;
}

// blend
float4 blend(float noiseValue, float2 texCoords)
{
	float4 base;
	
	if (blendType == opaqueBlend)
	{
		base = float4(noiseValue, noiseValue, noiseValue, 1);
	}
	else if (blendType == overlayBlend)
	{
		base = tex2D(baseSampler, texCoords);
		base.rgb *= noiseValue;
	}
	else if (blendType == additiveBlend)
	{
		base = tex2D(baseSampler, texCoords);
		base.rgb += noiseValue;
	}

	return base;
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
float getWorley(float2 texCoords, int feature, bool inverse)
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;
	
	// Calculate noise
	float2 coords = scaleTexCoords(texCoords) + baseValue * fbmOffset;
	return fbmWorley(coords, feature, inverse, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
}

// Vertex shader
void VSBase(inout float4 color:COLOR0, inout float2 texCoord:TEXCOORD0, inout float4 position:SV_Position) 
{ 
	position = mul(position, matrixTransform); 
}

// Perlin pixel shaders
float4 PSPerlin(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getPerlin(texCoords) * multiplier;
	float4 final = blend(value, texCoords);

	return final;
}

// Worley pixel shaders
float4 PSWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, worleyFeature, inverseWorley) * multiplier;
	float4 final = blend(value, texCoords);

	return final;
}

// Techniques
technique perlin_noise
{ 
	pass main 
	{ 
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSPerlin(); 
	}
}

// Worley techniques
technique worley_noise
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSWorley();
	}
}