using System;
using ModelMesh;

namespace ColladaImporter
{
	public interface IPoly
	{
		Vertex this [int index]
		{
			get; set;
		}
	}
	
	public class Triangle : IPoly
	{
		public Vertex p1, p2, p0;
	

		#region IPoly implementation
		public Vertex this[int index] {
			get {
				switch (index)
				{
				case 0:
					return p0;
				case 1: 
					return p1;
				case 2:
					return p2;
				default:
					throw new ArgumentException("index must be 0 - 2");
				}
			}
			set {
				switch (index)
				{
				case 0:
					p0 = value;
					break;
				case 1:
					p1 = value;
					break;
				case 2:
					p2 = value;
					break;				
				}
			}
		}
		#endregion
	}	
	
	public class Quad : IPoly
	{
		public Vertex p1, p2, p3, p0;
		#region IPoly implementation
		public Vertex this[int index] {
			get {
				switch (index)
				{
				case 0:
					return p0;
				case 1: 
					return p1;
				case 2:
					return p2;
				case 3:
					return p3;
				default:
					throw new ArgumentException("index must be 0 - 3");
				}
			}
			set {
				switch (index)
				{
				case 0:
					p0 = value;
					break;
				case 1:
					p1 = value;
					break;
				case 2:
					p2 = value;
					break;
				case 3:
					p3 = value;
					break;
				}
			}
		}
		#endregion
		
	}
	
	public class PrimitiveHelper
	{
		public static Triangle[] ToTriangles(Quad quad)
		{
			Triangle[] tris = new Triangle[2];
			
			VertexDeclaration quadDecl = quad.p0.VertexDeclaration;
			tris[0] = new Triangle();
			tris[1] = new Triangle();
			
			for (int i = 0; i < 3; ++i)
			{
				tris[0][i] = new Vertex(quadDecl);
				tris[1][i] = new Vertex(quadDecl);
			}			
			
			tris[0].p0.FromArray(quad[0].ToArray());
			tris[0].p1.FromArray(quad[1].ToArray());
			tris[0].p2.FromArray(quad[2].ToArray());
			
			tris[1].p0.FromArray(quad[0].ToArray());
			tris[1].p1.FromArray(quad[2].ToArray());
			tris[1].p2.FromArray(quad[3].ToArray());
			
			return tris;
		}
	}
}