#pragma once

#include "GL/glew.h"

#include "glm/glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtc/type_ptr.hpp"

#include <vector>
#include <stdint.h>

class Shader; // defines Shader class

class Mesh
{
public:
	Mesh();

	void CreateMesh(GLfloat* vertices, uint32_t vertexCout, uint32_t* indexs, uint32_t indexCount, Shader* useShader, uint8_t vertex_offset);
	void RenderMesh();

	void AddTexture(const char* textureLocation, GLenum color, bool flipTextureVerticaly);
	void AddTextureBuffer(GLuint& tex);
	void AddAttribPointer(uint8_t index, uint8_t size, GLenum type, GLboolean normalize, uint8_t offset, uint8_t position);
	void AddMaterial(const char* uniformAmbient, glm::vec3& ambient, const char* uniformDiffuse, glm::vec3& diffuse, const char* uniformSpecular, glm::vec3& specular, const char* uniformShininess, float shininess)
	{
		glUniform3fv(glGetUniformLocation(*shaderID, uniformAmbient), 1, glm::value_ptr(ambient));
		glUniform3fv(glGetUniformLocation(*shaderID, uniformDiffuse), 1, glm::value_ptr(diffuse));
		glUniform3fv(glGetUniformLocation(*shaderID, uniformSpecular), 1, glm::value_ptr(specular));
		glUniform1f(glGetUniformLocation(*shaderID, uniformShininess), shininess);
	}

	void SetPosition(glm::vec3& position);
	void SetRotation(glm::vec3& axis_to_rotate, GLfloat degree);
	void Scale(glm::vec3& scale);
	void SetMovement(glm::vec3& position_to_move);

	void SetMaterialAmbient(const char* uniformAmbient, glm::vec3& ambient)
	{
		glUniform3fv(glGetUniformLocation(*shaderID, uniformAmbient), 1, glm::value_ptr(ambient));
	}
	void SetMaterialDiffuse(const char* uniformDiffuse, glm::vec3& diffuse)
	{
		glUniform3fv(glGetUniformLocation(*shaderID, uniformDiffuse), 1, glm::value_ptr(diffuse));
	}
	void SetMaterialSpecular(const char* uniformSpecular, glm::vec3& specular)
	{
		glUniform3fv(glGetUniformLocation(*shaderID, uniformSpecular), 1, glm::value_ptr(specular));
	}
	void SetMaterialShininess(const char* uniformShininess, float shininess)
	{
		glUniform1f(glGetUniformLocation(*shaderID, uniformShininess), shininess);
	}

	// clears matrices
	void ClearTransofrmMatrix() { transform = glm::mat4(1); }
	void ClearRotationMatrix()  { rotation = glm::mat4(1); }
	void ClearScaleMatrix()		{ scale = glm::mat4(1); }
	void ClearAllMatrices() 
	{
		transform = glm::mat4(1);
		rotation = glm::mat4(1);
		scale = glm::mat4(1);
	}

	glm::vec3& GetPosition() { return position; }

	void ClearBuffer();
	~Mesh();

	////////////////////////////////////////////////////////////////////////////////
	void AddUniform3fv(const char* uniformLocation, glm::vec3& uniform)
	{
		glUniform3fv(glGetUniformLocation(*shaderID, uniformLocation), 1, glm::value_ptr(uniform));
	}
	void AddUniform1i(const char* uniformLocation, uint16_t uniform)
	{
		glUniform1i(glGetUniformLocation(*shaderID, uniformLocation), uniform);
	}
	void AddUniform1f(const char* uniformLocation, GLfloat uniform)
	{
		glUniform1f(glGetUniformLocation(*shaderID, uniformLocation), uniform);
	}
	////////////////////////////////////////////////////////////////////////////////


private:
	GLuint VAO, VBO, IBO, indexCount, texture;
	uint16_t* shaderID;
	std::vector<GLuint> textures;
	uint8_t textureCount = 0;

	uint32_t vertexCount;

	glm::mat4 transform = glm::mat4(1);
	glm::mat4 rotation = glm::mat4(1);
	glm::mat4 scale = glm::mat4(1);
	glm::vec3 position;

};
