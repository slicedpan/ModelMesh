using System;

namespace ColladaImporter
{
	public static class ArrayExtensions
	{
		public static void CopyTo(this float[] array, float[] dest, int sourceOffset, int destOffset)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				dest[i + destOffset] = array[i + sourceOffset];	
			}		
		}
	}
}

