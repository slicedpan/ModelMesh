using System;	
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using ModelMesh;

namespace ColladaImporter
{
	public class ColladaXML
	{
		private Mesh _mesh;
		public Mesh Mesh
		{
			get { return _mesh; }
		}
		XmlNamespaceManager nsManager;
		
		float UnitConversionFactor = 1.0f;
		
		public ColladaXML (string schemaFileName)
		{			
			_mesh = new Mesh();		
		}
		
		public float GetConversionFactor(string originalUnit)
		{
			switch (originalUnit.ToUpper())
			{
				case "METER":					
					return 1.0f;				
				case "METRE":
					return 1.0f;				
				//TODO Add More
			}
			return 1.0f;
		}
		
		public void ParseGlobals(XPathNavigator nav)
		{
			Console.WriteLine("parsing Globals...");
			Console.WriteLine(nav.LocalName);
			
			var unitNode = nav.SelectSingleNode("c:unit", nsManager);
			
			if (unitNode != null)
			{
				string unitName = unitNode.GetAttribute("name", unitNode.GetNamespace("c"));
				float baseFactor;
				if (!float.TryParse(unitNode.GetAttribute(unitName, unitNode.GetNamespace("c")), out baseFactor))
					throw new GeometryParserException("could not parse unit type!");
				UnitConversionFactor = GetConversionFactor(unitName) * baseFactor;
				Console.WriteLine("Units in file: " + unitName + ", Unit conversion Factor: " + UnitConversionFactor.ToString());
			}		
			else
			{
				Console.WriteLine("No unit info found.");
			}

		}
		
		public bool Parse (string XmlFileName)
		{		
			bool failed = false;
			XmlDocument doc = new XmlDocument();
			doc.Load(XmlFileName);		
			nsManager = new XmlNamespaceManager(doc.NameTable);
			nsManager.AddNamespace("c", "http://www.collada.org/2005/11/COLLADASchema");
			
			XPathNavigator nav = doc.CreateNavigator();
			
			var asset = nav.SelectSingleNode("/c:COLLADA/c:asset", nsManager);
			ParseGlobals(asset);
			
			var geometries = nav.SelectSingleNode("/c:COLLADA/c:library_geometries", nsManager);
			ParseGeometryLibrary(geometries);
			
			
			if (failed)
				return false;
			
			return true;
		}		
		
		public void ParseGeometryLibrary(XPathNavigator nav)
		{
			var geometries = nav.Select("c:geometry", nsManager);
			if (geometries == null)
				throw new GeometryParserException("could not find any geometries!");
			while (geometries.MoveNext())
			{
				string geomName = geometries.Current.GetAttribute("id", geometries.Current.GetNamespace("c"));
				Console.WriteLine("parsing geometry: " + geomName);
				var geometryParser = new GeometryParser(nsManager);
				Mesh.Elements.Add(geometryParser.Parse(geometries.Current, geomName, true));				
			}
		}
		
		
	}
}
