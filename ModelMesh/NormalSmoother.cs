using System;
using System.Collections.Generic;

namespace ModelMesh
{
	public class NormalSmoother : IMeshOptimiser
	{
		float[] _optimisedVertexBuffer;
		ushort[] _optimisedIndexBuffer;	
		ushort[] _originalIndexBuffer;
		MeshElement _meshElement;
		
		VertexDeclaration _vertexDeclaration;		
		List<VertexChannel> unsmoothedChannels;		
		
		#region IMeshOptimiser implementation
		void IMeshOptimiser.Apply (float[] originalVertexBuffer, ushort[] originalIndexBuffer, MeshElement meshElement)
		{
			_vertexDeclaration = meshElement.VertexDeclaration;	
			_meshElement = meshElement;
			_originalIndexBuffer = originalIndexBuffer;
			unsmoothedChannels = new List<VertexChannel>();
			Dictionary<int, List<int>> smoothingCandidates = new Dictionary<int, List<int>>();
			
			foreach (VertexChannel channel in _vertexDeclaration.channels)
			{
				if (channel.Name != "NORMAL")
					unsmoothedChannels.Add(channel);
			}
			
			List<int> uniqueVertices = new List<int>(meshElement.VertexCount);
			int smoothCount = 0;
			
			for (int i = 0; i < meshElement.VertexCount; ++i)
			{
				int matching = -1;
				foreach (int j in uniqueVertices)
				{
					if (ChannelsMatch(i, j))
					{
						matching = j;
						break;
					}
				}
				if (matching >= 0)
				{	
					if (smoothingCandidates.ContainsKey(matching))
					{
						smoothingCandidates[matching].Add(i);
					}
					else
					{
						smoothingCandidates.Add(matching, new List<int>());
						smoothingCandidates[matching].Add(i);
					}
					smoothCount++;
				}
				else
				{
					uniqueVertices.Add(i);
				}
			}
			
			_optimisedVertexBuffer = new float[uniqueVertices.Count * _vertexDeclaration.Stride];
			_optimisedIndexBuffer = new ushort[originalIndexBuffer.Length];
			int count = 0;
			
			for (int i = 0; i < uniqueVertices.Count; ++i)
			{				
				MeshHelper.CopyFloatArray(originalVertexBuffer, uniqueVertices[i] * _vertexDeclaration.Stride, _optimisedVertexBuffer, i * _vertexDeclaration.Stride, _vertexDeclaration.Stride);
				ChangeIndices(uniqueVertices[i], i);
				foreach (int j in smoothingCandidates[uniqueVertices[i]])
				{
					float[] normal = meshElement.ReadAttribute("NORMAL", j);
					MeshHelper.AddFloatArray(normal, 0, _optimisedVertexBuffer, i * _vertexDeclaration.Stride + _vertexDeclaration.GetChannel("NORMAL").Offset, 3);
					ChangeIndices(j, i);
					++count;
				}
				MeshHelper.Normalize(_optimisedVertexBuffer, i * _vertexDeclaration.Stride + _vertexDeclaration.GetChannel("NORMAL").Offset, 3);
			}
			
		}
		
		void ChangeIndices (int oldIndex , int newIndex)
		{
			for (int i = 0; i < _optimisedIndexBuffer.Length; ++i)
			{
				if (_originalIndexBuffer[i] == (ushort)oldIndex)
				{
					_optimisedIndexBuffer[i] = (ushort)newIndex;
					return;
				}
			}
		}
		
		bool ChannelsMatch(int index1, int index2)
		{
			foreach (VertexChannel channel in unsmoothedChannels)
			{
				if (MeshHelper.GetDist(_meshElement.ReadAttribute(channel.Name, index1), _meshElement.ReadAttribute(channel.Name, index2)) > 0.0001f)
				{
					return false;
				}
			}
			return true;
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

