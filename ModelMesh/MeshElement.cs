using System;
using OpenTK;
namespace ModelMesh
{
	/// <summary>
	/// Each MeshElement in a Mesh has one shader and one transform associated with it. 
	/// </summary>
	public class MeshElement
	{
		Matrix4 Transform;
		public Dictionary<string, Shader> Shaders;
		float[] _vertexBuffer;
		ushort[] _indexBuffer;
		VertexDeclaration _vertexDeclaration;
		
		#region properties
		
		public float[] VertexBuffer
		{
			get { return _vertexBuffer; }
		}
		public ushort[] indexBuffer
		{
			get { return _indexBuffer; }
		}
		public VertexDeclaration VertexDeclaration
		{
			get { return _vertexDeclaration; }
		}
		
		#endregion
		
		public MeshElement (VertexDeclaration vertexDeclaration, float[] vertexBuffer, ushort[] indexBuffer)
		{
			_vertexDeclaration = vertexDeclaration;
			_vertexBuffer = vertexBuffer;
			_indexBuffer = indexBuffer;
		}
	}
}

