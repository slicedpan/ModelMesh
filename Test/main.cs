using System;
using ModelMesh;
using ColladaImporter;

namespace Test
{
	public class main
	{
		public static void Main ()
		{
			ColladaXML daeReader = new ColladaXML(" ");
			daeReader.Parse("test.dae");
			daeReader.Mesh.CreateGPUBuffers();
		}
	}
}

