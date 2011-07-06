using System;
using OpenTK;
using System.Collections.Generic;

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
		
		public void Optimise(IMeshOptimiser meshOptimiser)
		{
			meshOptimiser.Apply(_vertexBuffer, _indexBuffer);
			_vertexBuffer = meshOptimiser.optimisedVertexBuffer;
			_indexBuffer = meshOptimiser.optimisedIndexBuffer;
		}
		
		public MeshElement (VertexDeclaration vertexDeclaration, float[] vertexBuffer, ushort[] indexBuffer)
		{
			_vertexDeclaration = vertexDeclaration;
			_vertexBuffer = vertexBuffer;
			_indexBuffer = indexBuffer;
		}		
		/// <summary>
		/// Reads an attribute from the vertex buffer.
		/// </summary>
		/// <param name="attributeName">
		/// A <see cref="System.String"/> that describes the vertex attribute ("VERTEX", "NORMAL" etc..).
		/// </param>
		/// <param name="element">
		/// <see cref="System.Int32"/>, the vertex you want to read the data from. 
		/// </param>
		/// <returns>
		/// A <see cref="System.Single[]"/> that stores the attribute data.
		/// </returns>
		public float[] ReadAttribute(string attributeName, int element)
		{
			var channel = _vertexDeclaration.GetChannel(attributeName);
			float[] array = new float[channel.Stride];
			for (int i = 0; i < channel.Stride; ++i)
			{
				array[i] = _vertexBuffer[element * _vertexDeclaration.Stride + channel.Offset + i];
			}
			return array;
		}
		/// <summary>
		/// Reads a range of attributes from the vertex buffer.
		/// </summary>
		/// <param name="attributeName">
		/// A <see cref="System.String"/> that describes the vertex attribute ("VERTEX", "NORMAL" etc..).
		/// </param>
		/// <param name="startElement">
		/// <see cref="System.Int32"/>, the vertex you want to start reading data from.
		/// </param>
		/// <param name="count">
		/// <see cref="System.Int32"/>, number of vertex attributes you want to read.
		/// </param>
		/// <returns>
		/// A <see cref="System.Single[]"/> that contains all the attribute data.
		/// </returns>
		public float[] ReadAttributeRange(string attributeName, int startElement, int count)
		{
			var channel = _vertexDeclaration.GetChannel(attributeName);
			float[] array = new float[channel.Stride * count];
			for (int i = 0; i < count; ++i)
			{
				ReadAttribute(attributeName, startElement + i).CopyTo(array, i * channel.Stride);
			}
			return array;
		}
		/// <summary>
		/// Writes an attribute to a single vertex.
		/// </summary>
		/// <param name="sourceArray">
		/// <see cref="System.Single[]"/>, the source data. (use Vector2.ToArray() for example).
		/// </param>
		/// <param name="sourceOffset">
		/// <see cref="System.Int32"/>, the position in the source data you want to start reading data from.
		/// </param>
		/// <param name="attributeName">
		/// A <see cref="System.String"/>, that describes the vertex attribute ("VERTEX", "NORMAL" etc..).
		/// </param>
		/// <param name="element">
		/// <see cref="System.Int32"/>, the first vertex you want to start writing to.
		/// </param>
		public void WriteAttribute(float[] sourceArray, int sourceOffset, string attributeName, int element)
		{
			var channel = _vertexDeclaration.GetChannel(attributeName);
			for (int i = 0; i < channel.Stride; ++i)
			{
				_vertexBuffer[element * _vertexDeclaration.Stride + channel.Offset + i] = sourceArray[i + sourceOffset];
			}
		}
		/// <summary>
		/// Writes an attribute to a single vertex.
		/// </summary>
		/// <param name="sourceArray">
		/// <see cref="System.Single[]"/>, the source data. (use Vector2.ToArray() for example).
		/// </param>
		/// <param name="attributeName">
		/// A <see cref="System.String"/> that describes the vertex attribute ("VERTEX", "NORMAL" etc..).
		/// </param>
		/// <param name="element">
		/// <see cref="System.Int32"/>, the first vertex you want to start writing to.
		/// </param>
		public void WriteAttribute(float[] sourceArray, string attributeName, int element)
		{
			WriteAttribute(sourceArray, 0, attributeName, element);	
		}
		/// <summary>
		/// Write a range of vertex attributes to the vertex buffer.
		/// </summary>
		/// <param name="sourceArray">
		/// <see cref="System.Single[]"/>, the source Data.
		/// </param>
		/// <param name="attributeName">
		/// A <see cref="System.String"/> that describes the vertex attribute ("VERTEX", "NORMAL" etc..).
		/// </param>
		/// <param name="startElement">
		/// <see cref="System.Int32"/>, the first vertex to write to.
		/// </param>
		/// <param name="count">
		/// <see cref="System.Int32"/>, the number of vertices to write.
		/// </param>
		public void WriteAttributeRange(float[] sourceArray, string attributeName, int startElement, int count)
		{
			var channel = _vertexDeclaration.GetChannel(attributeName);
			for (int i = 0; i < count; ++i)
			{
				WriteAttribute(sourceArray, i * channel.Stride, attributeName, startElement + i);
			}
		}
	}
}

