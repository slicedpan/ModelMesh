using System;
using OpenTK;

namespace ModelMesh
{
	public class MeshHelper
	{
		public static float GetDist(float[] first, float[] second)
		{
			if (first.Length != second.Length)
				throw new ArgumentException("arrays must have same length!");
			float total = 0;
			for (int i = 0; i < first.Length; ++i)
			{
				total += (first[i] - second[i]) * (first[i] - second[i]);
			}
			return (float)Math.Sqrt((double)total);
		}
		public static void Normalize(float[] array, int position, int count)
		{
			float invLength = 0;			
			for (int i = 0; i < count; ++i)
			{
				invLength += array[position + i] * array[position + i];
			}
			invLength = (float)Math.Sqrt((double)invLength);
			for (int i = 0; i < count; ++i)
			{
				array[position + i] /= invLength;
			}
		}
		public static void CopyFloatArray(float[] source, int sourceOffset, float[] dest, int destOffset, int count)
		{
			for (int i = 0; i < count; ++i)
			{
				dest[i + destOffset] = source[i + sourceOffset];
			}
		}
		
		public static void AddFloatArray(float[] source, int sourceOffset, float[] dest, int destOffset, int count)
		{
			for (int i = 0; i < count; ++i)
			{
				dest[i + destOffset] += source[i + sourceOffset];
			}
		}
	}
}

