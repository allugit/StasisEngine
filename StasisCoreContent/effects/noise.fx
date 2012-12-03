#include <noise_functions.fx>

sampler baseSampler : register(s0);

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
float fbmScale;
float4 noiseLowColor;
float4 noiseHighColor;
int fbmIterations;

float4x4 matrixTransform;

// Vertex shader
void VSBase(inout float4 color:COLOR0, inout float2 texCoord:TEXCOORD0, inout float4 position:SV_Position) 
{ 
	position = mul(position, matrixTransform); 
}

// Pixel shader
float4 PSBaseNoise(float2 texCoords:TEXCOORD0) : COLOR0
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;

	// Set position
	float2 p = (offset / renderSize) - (texCoords * aspectRatio) / noiseScale;

	// Calculate noise
	float2 coords = p + baseValue * fbmOffset;
	float value = 0;
	if (fbmPerlinBasis)
		value = fbmPerlin(coords, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
	else if (fbmCellBasis)
		value = fbmWorley(coords, false, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
	else
		value = fbmWorley(coords, true, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
	
	base.rgb *= value * multiplier;

	return base;
}

technique generic
{
    pass layered
    {
		VertexShader = compile vs_3_0 VSBase();
        PixelShader = compile ps_3_0 PSBaseNoise();
    }
}
