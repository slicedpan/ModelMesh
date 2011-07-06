using System;
using System.Collections.Generic;

namespace ModelMesh
{
	public class Shader
	{
		protected Dictionary<string, int> _uniforms;
		protected VertexDeclaration _vertexDeclaration;
		public virtual void Use()
		{
			
		}
		public virtual Dictionary<string, int> Uniforms
		{
			get { return _uniforms; }
		}
		public virtual VertexDeclaration VertexDeclaration
		{
			get { return _vertexDeclaration; }
		}
	}
}

