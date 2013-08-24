sampler baseSample : register(s0);
sampler postSource : register(s1);
float2 renderSize;

float4 LiquidPS(float2 texCoord:TEXCOORD0) : COLOR0
{
	// Base texture has density information in the red channel,
	//   and x and y velocity in the green and blue channels.
    float4 base = tex2D(baseSample, texCoord);
	float alpha = step(0.5, base.a) * 0.8;
	float3 blue = float3(0.07, 0.15, 0.4);
	float4 result = float4(1, 1, 1, alpha);

	// Calculate speed
	float speed = sqrt(base.g * base.g + base.b * base.b) * 0.75;
	if (base.a > 0.9)
		speed *= 2;
	else if (base.a > 0.7 && base.a < 0.8)
		speed *= 0.5;
	else
		speed = 0;

	// Warp post source
	result.rgb *= tex2D(postSource, texCoord + float2(-speed, -speed) / 10).rgb;

	// Pixel size
	float2 pixelSize = 1 / renderSize;

	// Top shimmer
	float top = base.a;
	top -= tex2D(baseSample, texCoord + float2(0, -6.5 * pixelSize.y)).a;
	top = top > 0.7 ? top : 0;
	result.rgb += top;

	// Clamp result based on alpha
	result *= step(0.7, base.a);

	// Apply speed
	result.rgb += speed;

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

	// Add some blue
	result.rgb += blue * alpha;

	return result;
}

technique Main
{
    pass Base
    {
        PixelShader = compile ps_2_0 LiquidPS();
    }
}
