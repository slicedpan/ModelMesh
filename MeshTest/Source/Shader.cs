using System;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Collections.Generic;

namespace tkglengine
{
	public class Shader
	{
		int _glHandle;
		int vertexHandle;
		int fragHandle;
		
		public int Handle
		{
			get {return _glHandle;}
		}
		

		public Dictionary <string, int> uniforms;	
		
		public Shader (string vertexFilename, string fragmentFilename)
		{
			
			uniforms = new Dictionary<string, int>();
			
			vertexHandle = GL.CreateShader(ShaderType.VertexShader);
			var vertexSource = File.ReadAllText(Paths.ShaderPath + vertexFilename);
			
			GL.ShaderSource(vertexHandle, vertexSource);
			
			fragHandle = GL.CreateShader(ShaderType.FragmentShader);
			var fragSource = File.ReadAllText(Paths.ShaderPath + fragmentFilename);
			GL.ShaderSource(fragHandle, fragSource);
			
			GL.CompileShader(vertexHandle);	
			Console.WriteLine(GL.GetShaderInfoLog(vertexHandle));			
			
			GL.CompileShader(fragHandle);
			Console.WriteLine(GL.GetShaderInfoLog(fragHandle));
			
			_glHandle = GL.CreateProgram();
			GL.AttachShader(_glHandle, vertexHandle);
			GL.AttachShader(_glHandle, fragHandle);
			GL.LinkProgram(_glHandle);
			
			Console.WriteLine(GL.GetProgramInfoLog(_glHandle));	
			
			int size;
			ActiveUniformType type;
			int uniformCount;
			 
			GL.GetProgram(_glHandle, ProgramParameter.ActiveUniforms, out uniformCount);
			Console.WriteLine(uniformCount.ToString() + " active uniforms");
			for(int i = 0; i < uniformCount; ++i)
			{
			    string name = GL.GetActiveUniform(_glHandle, i, out size, out type);
			    int slot = GL.GetUniformLocation(_glHandle, name);
				uniforms.Add(name, slot);
			}			
		}	
	}
}
