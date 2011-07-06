using System;
using ModelMesh;

namespace ColladaImporter
{
	public class Vertex
	{
		VertexDeclaration _vertexDeclaration;		
		float[] _data;
		
		public VertexDeclaration VertexDeclaration
		{
			get { return _vertexDeclaration; }
		}
		
		public float[] Data
		{
			get { return _data; }
		}
		
		public Vertex (VertexDeclaration declaration)
		{
			_vertexDeclaration = declaration;
			_data = new float[_vertexDeclaration.Stride];
		}
		public void FromArray(float[] array)
		{
			FromArray(array, 0);
		}
		public void FromArray(float[] array, int offset)
		{
			if (array.Length < offset + _vertexDeclaration.Stride)
				throw new ArgumentException("array does not have enough elements!");
			array.CopyTo(_data, offset, 0);
		}
		public float[] ToArray()
		{
			return _data;
		}
	}
}


