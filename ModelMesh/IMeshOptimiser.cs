using System;

namespace ModelMesh
{
	public interface IMeshOptimiser
	{
		void Apply(float[] originalVertexBuffer, ushort[] originalIndexBuffer, MeshElement meshElement);
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

