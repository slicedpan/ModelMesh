#version 330

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 TexCoord;

uniform mat4 WVP;

out vec4 normal;
out vec4 vPos;
out vec2 texCoord;

void main()
{		
	normal = vec4(Normal, 1.0);
	vPos = vec4(Position, 1.0);
	texCoord = TexCoord;
	gl_Position = WVP * vec4(Position, 1.0);
}
