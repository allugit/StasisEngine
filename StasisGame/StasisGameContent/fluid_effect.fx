sampler baseSample : register(s0);
float2 renderSize;

float4 LiquidPS(float2 texCoord:TEXCOORD0) : COLOR0
{
	// Base texture has density information in the red channel,
	//   and x and y velocity in the green and blue channels.
    float4 base = tex2D(baseSample, texCoord);
	float4 blue = float4(0.15, 0.3, 0.6, clamp(base.a, 0, 0.5));
	float4 result = blue;

	// Pixel size
	float2 pixelSize = 1 / renderSize;

	// Top shimmer
	float top = base.a;
	top -= tex2D(baseSample, texCoord + float2(0, -8 * pixelSize.y)).a;
	top = top > 0.7 ? top : 0;
	result.rgb += top;

	// Clamp result based on alpha
	result *= step(0.7, base.a);

	// Speed
	float speed = sqrt(base.g * base.g + base.b * base.b) * 0.75;
	if (base.a > 0.9)
		result.rgb += speed * 2;
	else if(base.a > 0.8)
		result.rgb += speed;
	else if(base.a > 0.7)
		result.rgb += speed * 0.5;
	else
		result.rgb = 0;

	// Scattering
	float scattering = base.a / 10;
	scattering += tex2D(baseSample, texCoord + float2(1, -10) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(-2, -20) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(3, -30) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(-4, -40) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(5, -50) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(-6, -60) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(7, -70) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(-8, -80) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(9, -90) * pixelSize) / 10;
	scattering += tex2D(baseSample, texCoord + float2(-10, -100) * pixelSize) / 10;
	scattering = 1 - scattering;
	scattering = lerp(0.5, 0.8, scattering);
	result.rgb *= scattering;

	return result;
}

technique Main
{
    pass Base
    {
        PixelShader = compile ps_2_0 LiquidPS();
    }
}
