sampler textureSampler : register(s0);
float4x4 world;
float4x4 view;
float4x4 projection;
float4x4 texCoordTransform;

struct VSInput
{
    float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
	float3 color : COLOR0;
};

struct VSOutput
{
    float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
	float3 color : COLOR0;
};

///////////////////////////////////////////////
// Vertex shader
///////////////////////////////////////////////
VSOutput VSDrawPrimitives(VSInput input)
{
    VSOutput output;

    float4 worldPosition = mul(input.position, world);
    float4 viewPosition = mul(worldPosition, view);
    output.position = mul(viewPosition, projection);
	output.texCoord = input.texCoord;
	output.color = input.color;
	//output.texCoord = mul(input.texCoord, texCoordTransform);

    return output;
}

///////////////////////////////////////////////
// Pixel shader -- White primitives
///////////////////////////////////////////////
float4 PSDrawPrimitives(VSOutput input) : COLOR0
{
	return float4(1, 1, 1, 1);
}

///////////////////////////////////////////////
// Pixel shader -- Textured primitives
///////////////////////////////////////////////
float4 PSDrawTexturedPrimitives(VSOutput input) : COLOR0
{
	float4 tex = tex2D(textureSampler, input.texCoord);
	tex.rgb *= input.color;
	return tex;
}

//////////////////////////////////////////////
// Techniques
//////////////////////////////////////////////
technique generic
{
    pass primitives
    {
        VertexShader = compile vs_2_0 VSDrawPrimitives();
        PixelShader = compile ps_2_0 PSDrawPrimitives();
    }

	pass textured_primitives
	{
		VertexShader = compile vs_2_0 VSDrawPrimitives();
		PixelShader = compile ps_2_0 PSDrawTexturedPrimitives();
	}
}
