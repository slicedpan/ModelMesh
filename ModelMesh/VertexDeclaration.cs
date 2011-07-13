
using System;
using System.Runtime.InteropServices;
using OpenTK;
using System.Collections.Generic;

namespace ModelMesh
{	
	public interface IVertex
	{
		float[] ToArray();
		void FromArray(float[] array);
		void FromArray(float[] array, int offset);
		VertexDeclaration VertexDeclaration
		{
			get;
		}
	}	
	
	/// <summary>
	/// The VertexChannel class contains information about one specific vertex attribute, (e.g Position, Normal etc.).
	/// </summary>
	public class VertexChannel
	{
		
		#region fields
		int _stride;
		int _offset;
		string _name;
		#endregion
		
		#region properties
		public int Stride
		{
			get { return _stride; }
		}
		public int Offset
		{
			get { return _offset; }			
		}
		public string Name
		{
			get { return _name; }
		}
		#endregion
		
		public VertexChannel(string name, int offset, int stride)
		{
			_stride = stride;
			_offset = offset;
			_name = name;
		}
	}
	/// <summary>
	/// VertexDeclaration class is a container class for vertex attribute information. The default constructor takes any number of <see cref="VertexChannel"/> arguments. The stride is automatically calculated.
	/// </summary>  
	public class VertexDeclaration
	{
		public List<VertexChannel> channels;
		
		int _stride = 0;
		public int Stride
		{
			get { return _stride; }
		}
		
		#region static declarations
		
		static VertexDeclaration _vertexPosition;
		static VertexDeclaration _vertexPositionNormal;
		static VertexDeclaration _vertexPositionNormalTexture;
		
		public static VertexDeclaration Position
		{
			get 
			{
				if (_vertexPosition == null)					
					_vertexPosition = new VertexDeclaration(new VertexChannel("VERTEX", 0, 3));
				return _vertexPosition;
			}
		}
		public static VertexDeclaration PositionNormal
		{
			get 
			{
				if (_vertexPositionNormal == null)
					_vertexPositionNormal = new VertexDeclaration(new VertexChannel("VERTEX", 0, 3), new VertexChannel("NORMAL", 3, 3));
				return _vertexPositionNormal;
			}
		}
		public static VertexDeclaration PositionNormalTexture
		{
			get 
			{
				if (_vertexPositionNormalTexture == null)
					_vertexPositionNormalTexture = new VertexDeclaration(new VertexChannel("VERTEX", 0, 3), new VertexChannel("NORMAL", 3, 3), new VertexChannel("TEXCOORD", 6, 2));
				return _vertexPositionNormalTexture;
			}
		}
		#endregion
		
		public bool ContainsChannel(string name)
		{
			foreach (VertexChannel channel in channels)
			{
				if (channel.Name == name)
					return true;
			}
			return false;
		}
		
		/// <summary>
		/// Creates a VertexDeclaration object from vertex channels.
		/// </summary>
		/// <param name="vertexChannels">
		/// A list of <see cref="VertexChannel"/> objects.
		/// </param>
		public VertexDeclaration(params VertexChannel[] vertexChannels)
		{
			channels = new List<VertexChannel>();
			foreach (VertexChannel channel in vertexChannels)
			{
				channels.Add(channel);
				_stride += channel.Stride;
			}
		}
		/// <summary>
		/// Creates a VertexDeclaration object from vertex channels.
		/// </summary>
		/// <param name="vertexChannels">
		/// A <see cref="List<VertexChannel>"/> containing the vertex channels that constitute the vertex declaration.
		/// </param>
		public VertexDeclaration(List<VertexChannel> vertexChannels)
		{
			channels = new List<VertexChannel>();
			foreach (VertexChannel channel in vertexChannels)
			{
				channels.Add(channel);
				_stride += channel.Stride;
			}
		}
		public VertexChannel GetChannel(string channelName)
		{
			int channelIndex = -1;
			for (int i = 0; i < channels.Count; ++i)
			{
				if (channelName == channels[i].Name)
				{
					channelIndex = i;
					break;
				}
			}
			if (channelIndex < 0)
				throw new Exception("vertex channel: " + channelName + " not found in vertex declaration");
			return channels[channelIndex];
		}
	}
	
	public struct VertexPositionNormal : IVertex
	{		
		public Vector3 Position;
		public Vector3 Normal;
				
		#region IVertex implementation
		float[] IVertex.ToArray ()
		{
			float[] array = new float[6];
			array[0] = Position.X;
			array[1] = Position.Y;
			array[2] = Position.Z;
			array[3] = Normal.X;
			array[4] = Normal.Y;
			array[5] = Normal.Z;
			return array;
		}
		
		void IVertex.FromArray(float[] array)
		{
			if (array.Length < 6)
				throw new ArgumentException("Array does not have enough elements!");
			Position.X = array[0];
			Position.Y = array[1];
			Position.Z = array[2];
			Normal.X = array[3];
			Normal.Y = array[4];
			Normal.Z = array[5];
		}

		VertexDeclaration IVertex.VertexDeclaration {
			get {
				return VertexDeclaration.PositionNormal;
			}			
		}
		




		void IVertex.FromArray (float[] array, int offset)
		{
			if (array.Length < 6 + offset)
				throw new ArgumentException("Array does not have enough elements!");
			Position.X = array[0 + offset];
			Position.Y = array[1 + offset];
			Position.Z = array[2 + offset];
			Normal.X = array[3 + offset];
			Normal.Y = array[4 + offset];
			Normal.Z = array[5 + offset];
		}

	#endregion
	}
	
	public struct VertexPositionNormalTexture : IVertex
	{		
		public Vector3 Position;
		public Vector3 Normal;		
		public Vector2 TexCoord;

		#region IVertex implementation
		float[] IVertex.ToArray ()
		{
			float[] array = new float[8];
			array[0] = Position.X;
			array[1] = Position.Y;
			array[2] = Position.Z;
			array[3] = Normal.X;
			array[4] = Normal.Y;
			array[5] = Normal.Z;
			array[6] = TexCoord.X;
			array[7] = TexCoord.Y;
			return array;
		}
		
		void IVertex.FromArray(float[] array)
		{
			if (array.Length < 8)
				throw new ArgumentException("Array does not have enough elements!");
			Position.X = array[0];
			Position.Y = array[1];
			Position.Z = array[2];
			Normal.X = array[3];
			Normal.Y = array[4];
			Normal.Z = array[5];
			TexCoord.X = array[6];
			TexCoord.Y = array[7];
		}
		
		void IVertex.FromArray(float[] array, int offset)
		{
			if (array.Length < 8 + offset)
				throw new ArgumentException("Array does not have enough elements!");
			Position.X = array[0 + offset];
			Position.Y = array[1 + offset];
			Position.Z = array[2 + offset];
			Normal.X = array[3 + offset];
			Normal.Y = array[4 + offset];
			Normal.Z = array[5 + offset];
			TexCoord.X = array[6 + offset];
			TexCoord.Y = array[7 + offset];
		}

		VertexDeclaration IVertex.VertexDeclaration {
			get {
				return VertexDeclaration.PositionNormalTexture;
			}			
		}
		#endregion
	}
	
	public struct VertexPosition : IVertex
	{
		public Vector3 Position;

		#region IVertex implementation
		void IVertex.FromArray (float[] array)
		{
			if (array.Length < 3)
				throw new ArgumentException("array does not have enough elements!");
			Position.X = array[0];
			Position.Y = array[1];
			Position.Z = array[2];
		}

		void IVertex.FromArray (float[] array, int offset)
		{
			throw new NotImplementedException ();
		}
		
		VertexDeclaration IVertex.VertexDeclaration
		{
			get
			{
				return VertexDeclaration.Position;
			}
		}
		
		float[] IVertex.ToArray()
		{
			float[] array = new float[3];
			array[0] = Position.X;
			array[1] = Position.Y;
			array[2] = Position.Z;
			return array;
		}

		#endregion
	}
}




