#version 330

out vec4 outputColor;

in vec4 normal;
in vec4 vPos;

uniform vec3 LightPos;

void main()
{
	vec3 lightVec = normalize(LightPos.xyz - vPos.xyz);
	float amount = clamp(dot(lightVec, normal.xyz), 0.0, 0.9);
	outputColor = vec4(amount * vec3(1.0), 1.0);
}
