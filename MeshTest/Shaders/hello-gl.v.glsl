#version 330

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 TexCoord;
layout(location = 3) in vec3 Tangent;
layout(location = 4) in vec3 BiTangent;

uniform mat4 WVP;

out vec3 normal;
out vec3 vPos;
out vec2 texCoord;
out vec3 tangent;
out vec3 bitangent;

void main()
{		
	normal = Normal;
	vPos = Position;
	texCoord = TexCoord;
	tangent = Tangent;
	bitangent = BiTangent;
	gl_Position = WVP * vec4(Position, 1.0);
}
