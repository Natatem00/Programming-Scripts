#version 330

layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 textur;
layout (location = 2) in vec3 norma;

out vec2 vertTexture;

//out vec4 vColor;
out vec3 Normals;
out vec3 FragPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = projection * view * model * vec4(pos, 1.0);
	FragPosition = vec3(model * vec4(pos, 1.0));																	// ues "FragPosition = vec3(projection * view * model * vec4(pos, 1.0));" if you use specular
	vertTexture = textur;

	//vColor = vec4(0.0, 0.0, 1.0, 0.1);
	Normals = mat3(transpose(inverse(model))) * norma;
}
