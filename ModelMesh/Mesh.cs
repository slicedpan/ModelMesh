using System;
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
	}
}

