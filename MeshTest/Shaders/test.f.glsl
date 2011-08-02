#version 330

out vec4 outputColor;

in vec3 normal;
in vec3 vPos;
in vec2 texCoord;
in vec3 tangent;
in vec3 bitangent;

uniform vec3 LightPos;
uniform vec3 LightColor;
uniform sampler2D diffuseMap;
uniform sampler2D normalMap;
uniform vec3 camPos;

void main()
{
	vec3 mapNormal = texture2D(normalMap, vec2(texCoord.x, texCoord.y * -1)).xyz;
	vec3 pixNormal = (mapNormal.x * tangent) + (mapNormal.y * bitangent) + (mapNormal.z * normal);
	pixNormal = normalize(pixNormal);
	
	vec3 lightVec = normalize(LightPos - vPos);
	vec3 lightAmount = clamp(dot(lightVec, pixNormal), 0.1, 1.0) * LightColor;	
	vec3 texColor = texture2D(diffuseMap, vec2(texCoord.x, texCoord.y * -1)).xyz;
	
	texColor *= lightAmount;
	
	vec3 camVec = normalize(vPos - camPos);
	
	float specamount = clamp(dot(reflect(lightVec, normal), camVec), 0.0, 1.0);
	vec3 specColor = pow(specamount, 15) * vec3(0, 1, 0);
	
	outputColor = vec4(texColor + specColor, 1.0);
	//outputColor = vec4(mapNormal, 1.0);
}
