#version 330

out vec4 FragColor;

in vec2 vertTexture;

uniform sampler2D texture1;


float offset = 1.0 / 500.0; // pixel's color offset
void main()
{
	vec2 offsets[9] = 
	{
		vec2(offset, offset),
		vec2(0, offset),
		vec2(-offset, offset),
		vec2(offset, 0),
		vec2(0, 0),
		vec2(-offset, 0),
		vec2(offset, -offset),
		vec2(0, -offset),
		vec2(-offset, -offset)
	};
	
	vec3 samples[9];
	// adds offset to current texture color
	for(int i = 0; i < 9; i++)
	{
		samples[i] = vec3(texture(texture1, vertTexture.st + offsets[i]));
	}
	
	float kernel[9] = {
		1, 1, 1,
		1,-8, 1,
		1, 1, 1
	};
	
	vec3 color = vec3(0);
	// multiplies by the kernel matrix
	for(int i = 0; i < 9; i++)
	{
		color += samples[i] * kernel[i];
	}
	
	FragColor = vec4(color * vec3(1, 5, 1), 1.0);
}