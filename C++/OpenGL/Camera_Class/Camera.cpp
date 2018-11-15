#include "Camera.h"
#include "Window.h"
#include "Time.h"


Camera::Camera(glm::vec3 position, float fov, Window* currentWindow)
{
	this->fov = fov;
	cameraPosition = position;
	this->currentWindow = currentWindow;

	view = glm::mat4(1);
	projection = glm::mat4(1);

	view = glm::translate(view, position);
	projection = glm::perspective(glm::radians(fov), currentWindow->GetASpect(), 0.1f, 100.f);
	glfwSetInputMode(currentWindow->GetCurrentWindow(), GLFW_CURSOR, GLFW_CURSOR_DISABLED);
	glfwSetWindowUserPointer(currentWindow->GetCurrentWindow(), this);
}

void Camera::ShowCamera(GLuint& shaderID)
{
	RotateCamera();
	MoveCamera();
	view = glm::lookAt(
		cameraPosition,
		cameraPosition + cameraForward,
		worldUp
		);
	glUniformMatrix4fv(glGetUniformLocation(shaderID, "view"), 1, GL_FALSE, glm::value_ptr(view));
	glUniformMatrix4fv(glGetUniformLocation(shaderID, "projection"), 1, GL_FALSE, glm::value_ptr(projection));
}

void Camera::MoveCamera()
{
	GLfloat* delta = &Time::GetDeltaTime();
	auto window = currentWindow->GetCurrentWindow();
	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
	{
		cameraPosition += cameraSpeed * (*delta) * cameraForward;
	}
	if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
	{
		cameraPosition -= cameraSpeed * (*delta) * cameraForward;
	}
	if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
	{
		cameraPosition -= cameraSpeed * (*delta) * glm::cross(cameraForward, worldUp);
	}
	if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
	{
		cameraPosition += cameraSpeed * (*delta) * glm::cross(cameraForward, worldUp);
	}
}

void Camera::RotateCamera()
{
	glfwSetCursorPosCallback(currentWindow->GetCurrentWindow(), mouse_position);
	glfwSetScrollCallback(currentWindow->GetCurrentWindow(), scroll_position);
}

void Camera::mouse_position(GLFWwindow* window, double mouseX, double mouseY)
{
	Camera* camera = static_cast<Camera*>(glfwGetWindowUserPointer(window));
	if (camera->firstMouse)
	{
		camera->lastX = mouseX;
		camera->lastY = mouseY;
		camera->firstMouse = false;
	}
	float offsetX = mouseX - camera->lastX;
	float offsetY = camera->lastY - mouseY;

	camera->lastX = mouseX;
	camera->lastY = mouseY;

	offsetX *= camera->mouseSensitivity;
	offsetY *= camera->mouseSensitivity;
	camera->pitch += offsetY;
	camera->yaw += offsetX;

	if (camera->pitch >= 89.0)
	{
		camera->pitch = 89.0;
	}
	if (camera->pitch <= -89.0)
	{
		camera->pitch = -89.0;
	}
	camera->cameraForward.x = cos(glm::radians(camera->yaw)) * cos(glm::radians(camera->pitch));
	camera->cameraForward.y = sin(glm::radians(camera->pitch));
	camera->cameraForward.z = sin(glm::radians(camera->yaw)) *cos(glm::radians(camera->pitch));
	glm::normalize(camera->cameraForward);
}

void Camera::scroll_position(GLFWwindow * window, double Xoffset, double Yoffset)
{
	Camera* camera = static_cast<Camera*>(glfwGetWindowUserPointer(window));
	if (camera->fov >= 1.0 && camera->fov <= 45.0)
	{
		camera->fov -= Yoffset;
	}
	if (camera->fov <= 1.0)
	{
		camera->fov = 1.0;
	}
	if (camera->fov >= 45.0)
	{
		camera->fov = 45.0;
	}
	camera->projection = glm::perspective(glm::radians(camera->fov), camera->currentWindow->GetASpect(), 0.1f, 100.f);
}

Camera::~Camera()
{
}
