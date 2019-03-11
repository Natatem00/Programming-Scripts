#pragma once
#include "Mesh.h"
#include "Shader.h"

#include "STB_IMAGE.h"

#include <string>

Mesh::Mesh()
{
	VAO = 0;
	VBO = 0;
	IBO = 0;
	indexCount = 0;
	texture = 0;

	textures.resize(1);
}

void Mesh::CreateMesh(GLfloat* vertices, uint32_t vertexCout, uint32_t* indexs, uint32_t indexCount, Shader* useShader, uint8_t vertex_offset)
{
	shaderID = &useShader->GetShaderId();
	this->indexCount = indexCount;
	vertexCount = vertexCout / vertex_offset;

	// gens VAO
	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);
	// if there are indexes
	if (indexCount != 0)
	{
		// gens IBO
		glGenBuffers(1, &IBO);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);

		// creates IBO data
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indexs[0]) * indexCount, indexs, GL_STATIC_DRAW);

	}
	// gens VBO
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);

	// creates VBO data
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices[0]) * vertexCout, vertices, GL_STATIC_DRAW);

	// unbinds VBO and VAO
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);
	// if there are indexes
	if (indexCount != 0)
	{
		// unbinds IBO
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	}
}

void Mesh::AddTexture(const char * textureLocation, GLenum color, bool flipTextureVerticaly)
{
	// loads texture data
	int32_t textureWidth, textureHeight, textureColorChannel;
	stbi_set_flip_vertically_on_load(flipTextureVerticaly);
	unsigned char* data = stbi_load(textureLocation, &textureWidth, &textureHeight, &textureColorChannel, 0);
	if (!data)
	{
		printf("Error: can't load texture data\n");
		return;
	}

	// generates texture
	glActiveTexture(GL_TEXTURE0 + textureCount);
	glGenTextures(1, &textures[textureCount]);

	glBindTexture(GL_TEXTURE_2D, textures[textureCount]);

	// texture's parameters
	glTextureParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);			// repeats texture's on X axis if texture coordinató is out of range(0-1)
	glTextureParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);			// repeats texture's on Y axis if texture coordinató is out of range(0-1)
	glTextureParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);				// blurs texture's edges if the mesh becomes smaller
	glTextureParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);				// blurs texture's edges if the mesh becomes bigger

	// creates texture
	glTexImage2D(GL_TEXTURE_2D, 0, color, textureWidth, textureHeight, 0, color, GL_UNSIGNED_BYTE, data);
	glGenerateMipmap(GL_TEXTURE_2D);
	textureCount++;
	textures.resize(1 + textureCount);

	// clear texture data
	stbi_image_free(data);
	glBindTexture(GL_TEXTURE_2D, 0);
}

void Mesh::AddTextureBuffer(GLuint& tex)
{
	textures[textureCount] = tex;
	textureCount++;
	textures.resize(1 + textureCount);
}

void Mesh::AddAttribPointer(uint8_t index, uint8_t size, GLenum type, GLboolean normalize, uint8_t offset, uint8_t position)
{
	glBindVertexArray(VAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glVertexAttribPointer(index, size, type, normalize, offset * sizeof(float), (void*)(position * sizeof(float)));
	glEnableVertexAttribArray(index);

	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);
}

void Mesh::SetPosition(glm::vec3& position)
{
	if (this->position != position)
	{
		transform = glm::translate(transform, position);
		this->position = position;
	}
}

void Mesh::SetRotation(glm::vec3& axis_to_rotate, GLfloat degree)
{
	rotation = glm::rotate(rotation, glm::radians(degree), axis_to_rotate);
}

void Mesh::Scale(glm::vec3& scale)
{
	this->scale = glm::mat4(1);
	this->scale = glm::scale(this->scale, scale);
}

void Mesh::SetMovement(glm::vec3& position_to_move)
{
	transform = glm::translate(transform, position);
	position = position;
}

void Mesh::RenderMesh()
{
	glBindVertexArray(VAO);

	for (size_t i = 0; i < textureCount; i++)
	{
		// activates mesh texture
		glActiveTexture(GL_TEXTURE0 + i);
		glBindTexture(GL_TEXTURE_2D, textures[i]);
	}

	/*----add here your uniform variables-----*/

	// you can transfer glGetUniformLocation(*shaderID, "model") to the CreateMesh() method 
	//and assign it to a global variable
	glUniformMatrix4fv(glGetUniformLocation(*shaderID, "model"), 1, GL_FALSE, glm::value_ptr(transform * rotation * scale));

	/*----------------------------------------*/

	// if there are indexes - draws them
	if (indexCount != 0)
	{
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
		glDrawElements(GL_TRIANGLES, indexCount, GL_UNSIGNED_INT, nullptr);
	}
	// if there are not indexes - draws vertices
	else
	{
		glDrawArrays(GL_TRIANGLES, 0, vertexCount);
	}
	
	// unbinds texture
	glBindTexture(GL_TEXTURE_2D, 0);
	glBindVertexArray(0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
}

void Mesh::ClearBuffer()
{
	if (VAO != 0)
	{
		glDeleteVertexArrays(1, &VAO);
		VAO = 0;
	}
	if (VBO != 0)
	{
		glDeleteBuffers(1, &VBO);
		VBO = 0;
	}
	if (IBO != 0)
	{
		glDeleteBuffers(1, &IBO);
		IBO = 0;
	}
	if(texture != 0)
	{
		glDeleteTextures(1, &texture);
	}
}

Mesh::~Mesh()
{
	ClearBuffer();
}

