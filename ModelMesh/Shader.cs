using System;
using System.Collections.Generic;
namespace ModelMesh
{
	public class Shader
	{
		public virtual void Use();		
		public virtual Dictionary<string, int> Uniforms
		{
			get;
		}
		public virtual VertexDeclaration VertexDeclaration
		{
			get;
		}
	}
}

