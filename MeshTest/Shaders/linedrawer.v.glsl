#version 330

layout(location = 0) in vec3 Position;

uniform mat4 WVP;


void main()
{	
	gl_Position = WVP * vec4(Position, 1.0);
}
