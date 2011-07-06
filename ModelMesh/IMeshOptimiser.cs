using System;
namespace ModelMesh
{
	public interface IMeshOptimiser
	{
		void Apply(float[] originalVertexBuffer, ushort[] originalIndexBuffer);
		float[] optimisedVertexBuffer
		{
			get;
		}
		ushort[] optimisedIndexBuffer
		{
			get;
		}
	}
}

