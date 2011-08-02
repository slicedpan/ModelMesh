#version 330

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec2 TexCoord;
layout(location = 3) in vec3 Tangent;
layout(location = 4) in vec3 BiTangent;

uniform mat4 WVP;
uniform sampler2D pixTex;
uniform int pixWidth;
uniform int textureWidth;
uniform int textureHeight;
uniform int offsetX;
uniform int offsetY;

out vec3 normal;
out vec3 vPos;
out vec2 texCoord;
out vec3 tangent;
out vec3 bitangent;
out vec3 color;

void main()
{		
	normal = Normal;
	float y = gl_InstanceID / pixWidth;
	float x = mod(gl_InstanceID, pixWidth);
	vec2 coord = vec2((x + offsetX) / textureWidth, (y + offsetY) / textureHeight);
	vec4 pixColor = texture2D(pixTex, coord);
	color = pixColor.xyz;
	vPos = Position + vec3(x, (1 - pixColor.w) * 1000, -y);
	texCoord = TexCoord;
	tangent = Tangent;
	bitangent = BiTangent;
	gl_Position = WVP * vec4(vPos, 1.0);
}
