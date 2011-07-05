using System;
using OpenTK;
using System.Collections.Generic;

namespace ModelMesh
{
	public static class VectorExtensions
	{
		
		public static float[] ToArray ( this Vector4 vec )
		{
			float[] array = new float[3];
			array[0] = vec.X;
			array[1] = vec.Y;
			array[2] = vec.Z;
			array[3] = vec.W;
			return array;
		}
		public static void FromArray ( this Vector4 vec, float[] array, int offset = 0 )
		{
			if (array.Length < 4 + offset)
				throw new ArgumentOutOfRangeException();
			
			vec.X = array[0 + offset];
			vec.Y = array[1 + offset];
			vec.Z = array[2 + offset];
			vec.W = array[3 + offset];
		}
		
		public static float[] ToArray ( this Vector3 vec )
		{
			float[] array = new float[3];
			array[0] = vec.X;
			array[1] = vec.Y;
			array[2] = vec.Z;
			return array;
		}
		public static void FromArray ( this Vector3 vec, float[] array, int offset = 0 )
		{
			if (array.Length < 3 + offset)
				throw new ArgumentOutOfRangeException();
			
			vec.X = array[0 + offset];
			vec.Y = array[1 + offset];
			vec.Z = array[2 + offset];
		}

		public static float[] ToArray ( this Vector2 vec )
		{
			float[] array = new float[2];
			array[0] = vec.X;
			array[1] = vec.Y;
			return array;
		}
		public static void FromArray ( this Vector2 vec, float[] array, int offset = 0 )
		{
			if (array.Length < 2 + offset)
				throw new ArgumentOutOfRangeException();
			
			vec.X = array[0 + offset];
			vec.Y = array[1 + offset];
		}
		public static List<Type> FloatArrayTypes()
		{
			var typeList = new List<Type>();
			typeList.Add(typeof(Vector2));
			typeList.Add(typeof(Vector2));
			typeList.Add(typeof(Vector2));
			return typeList;
		}
	}
}

