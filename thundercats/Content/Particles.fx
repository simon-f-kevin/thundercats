/* MIT License
*
* Copyright(c) 2016
* Rami Tabbara
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0

static const float PI = 3.14159265359;
static const float EIGHT_PI = 25.1327412287;


float Life;
float TotalTime;
int MaxNumOfParticles;
float2 RadiusRange;

matrix WorldViewProjection;

Texture2D ParticleTexture : register(t0);
sampler TexSampler : register(s0);

struct VSVertexInput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	uint InstanceId : SV_InstanceId;
};

struct VSInstanceInput
{
	float3 RandomIntervals : POSITION1;
	float ParticleTime : BLENDWEIGHT0;
};

struct VSOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

VSOutput MainVS(in VSVertexInput vertexInput, float3 RandomIntervals : POSITION1,
	float ParticleTime : BLENDWEIGHT0)
{
	VSOutput output = (VSOutput)0;

	// Scale the time to be between the interval 0 to 1
	// Note, we could pass in the pre-transformed interval to avoid this computation with
	// the vertex shader, but for the sake of clarity I've left it here
	float timeFraction = fmod((ParticleTime + TotalTime) / (float)Life, 1.0f);

	// We have a radial particle system
	// To be fancy, adjust the radius size by both the passed in random interval, time and instance id
	float radius = lerp(RadiusRange[0], RadiusRange[1], RandomIntervals.x)
		* timeFraction
		* ((vertexInput.InstanceId % 10) + 1) / 10;

	// Find relative position of particle
	float2 offset;
	sincos(EIGHT_PI * (vertexInput.InstanceId + 1) * 10 / MaxNumOfParticles, offset.x, offset.y);
	offset *= radius * sin(timeFraction * PI);

	// Calculate absolute position
	output.Position = vertexInput.Position;
	output.Position.xy += offset;
	output.Position.z += vertexInput.InstanceId * 10 / (float)MaxNumOfParticles;
	output.Position = mul(output.Position, WorldViewProjection);

	// Set colour of particle using the other two passed-in random intervals 
	float4 endColor = lerp(float4(0.0, 0.5, 1.0, 0.8), float4(1.0, 1.0, 1.0, 0.8), RandomIntervals.y);
	float4 startColor = lerp(float4(1.0, 0.0, 0.0, 0.8), float4(0.3, 0.0, 1.0, 1.0), RandomIntervals.z);

	output.Color = lerp(startColor, endColor, timeFraction);
	output.Color *= (-4 * pow(timeFraction - 0.5, 2) + 1);

	output.TexCoord = vertexInput.TexCoord;

	return output;
}

float4 MainPS(VSOutput input) : COLOR0
{
	return input.Color * ParticleTexture.Sample(TexSampler, input.TexCoord);
}

technique ParticleDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};