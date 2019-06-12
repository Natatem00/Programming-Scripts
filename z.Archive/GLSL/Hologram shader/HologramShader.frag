#version 330

/*struct Material
{
	sampler2D diffuseMap;
	sampler2D specularMap;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};*/

/*struct Light
{
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};*/

out vec4 FragColor;

in vec2 vertTexture;
in vec3 Normals;
in vec3 FragPosition;

uniform vec3 viewPosition; // camera position
uniform vec3 modelPosition;

//uniform Material material;
//uniform Light light;

void main()
{
		vec3 norm = normalize(Normals);
		vec3 lightPosition = modelPosition;
		vec3 lightDirection = normalize(lightPosition - FragPosition);

		// material
		vec3 materialAmbient = vec3(0.0, 0.98, 0.6);
		vec3 materialDiffuse = vec3(0.0, 0.5, 0.6);
		//vec3 materialSpecular = vec3(0.1, 0.5, 1);
		//float materialShininess = 0; 												// you can change this value to 0, if you want create blue hologram

		// light
		vec3 lightAmbient = vec3(0.0, 0.7, 0.6);
		vec3 lightDiffuse = vec3(0.5f, 0.5f, 0.5f);
		//vec3 lightSpecular = vec3(0.5, 0.5, 0.5);

		// ambient
		vec3 ambient = materialAmbient * lightAmbient;

		// diffuse
		float diff = max(dot(norm, lightDirection), 0.0);
		vec3 diffuse = (diff * materialDiffuse) * lightDiffuse;

		// specular
		//vec3 viewDirection = normalize(viewPosition - FragPosition);
		//vec3 reflectDirection = reflect(-lightDirection, norm);
		//float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), materialShininess);
		//vec3 specular = (spec * materialSpecular) * lightSpecular;

		vec3 result = (ambient + diffuse);										// add to result specular, if you want that the materialShininess value have sense

		FragColor = vec4(result, 0.4);

}
