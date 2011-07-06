using System;
using System.Collections.Generic;

namespace ModelMesh
{
	public class Mesh
	{
		List<MeshElement> _elements;
		public List<MeshElement> Elements
		{
			get { return _elements; }
		}
		public Mesh ()
		{
			_elements = new List<MeshElement>();
		}
		public void Optimise(IMeshOptimiser meshOptimiser)
		{
			foreach (MeshElement element in _elements)
			{
				element.Optimise(meshOptimiser);
			}
		}
	}
}

