using System;
namespace ModelMesh
{
	public class NormalSmoother : IMeshOptimiser
	{
		float[] _optimisedVertexBuffer;
		ushort[] _optimisedIndexBuffer;	

		#region IMeshOptimiser implementation
		void IMeshOptimiser.Apply (float[] originalVertexBuffer, ushort[] originalIndexBuffer)
		{

		}

		float[] IMeshOptimiser.optimisedVertexBuffer {
			get 
			{
				if (_optimisedVertexBuffer == null)
					throw new Exception("you must apply the optimiser before you can get the optimised buffers");
				return _optimisedVertexBuffer;
			}
		}

		ushort[] IMeshOptimiser.optimisedIndexBuffer {
			get 
			{
				if (_optimisedIndexBuffer == null)
					throw new Exception("you must apply the optimiser before you can get the optimised buffers");
				return _optimisedIndexBuffer;
			}
		}
		#endregion
}
}

