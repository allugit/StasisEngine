sampler baseSampler : register(s0);

float4 PixelShaderFunction(float2 texCoords:TEXCOORD0) : COLOR0
{
	float4 final = float4(0, 0, 0, 1);
	float4 antiFog = tex2D(baseSampler, texCoords);
	final.a = 1 - antiFog.r;

    return final;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
