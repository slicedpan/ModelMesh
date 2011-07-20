#version 330

out vec4 outputColor;

in vec4 normal;
in vec4 vPos;
in vec2 texCoord;

uniform vec3 LightPos;
uniform sampler2D diffuse;

void main()
{
	vec3 lightVec = normalize(LightPos.xyz - vPos.xyz);
	float amount = clamp(dot(lightVec, normal.xyz), 0.4, 0.9);	
	outputColor = vec4(amount * texture2D(diffuse, vec2(texCoord.x, texCoord.y * -1)));
}
