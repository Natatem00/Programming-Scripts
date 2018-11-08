#version 330

layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 textur;

out vec2 vertTexture;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = projection * view * model * vec4(pos, 1.0);
	
	vertTexture = textur;
}