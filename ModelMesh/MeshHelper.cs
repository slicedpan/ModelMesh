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
	}
}

