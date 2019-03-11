#pragma once

#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include "glm/glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtc/type_ptr.hpp"

class Window; // defines Window class

class Camera
{
public:
	Camera(glm::vec3 position, float fov, Window* currentWindow);

	void ShowCamera(GLuint& shaderID);

	glm::vec3& GetPosition() { return cameraPosition; }
	glm::vec3& GetDirection() { return cameraForward; }
	~Camera();

private:
	glm::mat4 view;
	glm::mat4 projection;

	glm::vec3 cameraPosition;
	glm::vec3 cameraForward = glm::vec3(0, 0, -1);
	glm::vec3 worldUp = glm::vec3(0, 1, 0);

	Window* currentWindow;

	GLfloat fov = 0;
	GLfloat cameraSpeed = 5.f;
	GLfloat lastX = 400;
	GLfloat lastY = 300;
	GLfloat pitch = 0;
	GLfloat yaw = 270;
	GLfloat mouseSensitivity = 0.5f;
	bool firstMouse = true;

	void MoveCamera();
	void RotateCamera();

	static void mouse_position(GLFWwindow* window, double mouseX, double mouseY);
	static void scroll_position(GLFWwindow* window, double Xoffset, double Yoffset);
};

