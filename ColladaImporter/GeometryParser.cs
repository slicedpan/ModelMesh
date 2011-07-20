using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.XPath;
using ModelMesh;
using OpenTK;

namespace ColladaImporter
{
	public class GeometryParserException : Exception
	{
		public GeometryParserException(string message) :
			base(message)
		{}	
	}
	public class GeometryParser
	{
		#region fields
		float[] vertexData;
		ushort[] indexData;
		int[] indices;
		Dictionary <string, Source> sources;
		Dictionary <string, Semantic> semantics;
		string nsName;
		VertexDeclaration _vertexDeclaration;
		int[] polyVertexCount;
		public String Name;
		XmlNamespaceManager nsManager;
		int triCount = 0;
		Triangle[] tris;
		//int stride;
		#endregion
		
		public VertexDeclaration VertexDeclaration
		{
			get 
			{
				return _vertexDeclaration;
			}
		}
		
		
		public GeometryParser (XmlNamespaceManager pNSManager)			
		{			
			nsManager = pNSManager;
			sources = new Dictionary<string, Source>();
			semantics = new Dictionary<string, Semantic>();			
		}
		
		private float[] GenerateTangents(Triangle tri, int vertexNum)
		{
			int posOffset = _vertexDeclaration.GetChannel("VERTEX").Offset;
			int texOffset = _vertexDeclaration.GetChannel("TEXCOORD").Offset;
			
			int p1 = (vertexNum + 1) % 3;
			int p2 = (vertexNum + 2) % 3;
			
			Vector3 vec1 = VectorMethods.FromArray3(tri[p1].Data, posOffset);
			vec1 -= VectorMethods.FromArray3(tri[vertexNum].Data, posOffset);
			Vector3 vec2 = VectorMethods.FromArray3(tri[p1].Data, posOffset);
			vec2 -= VectorMethods.FromArray3(tri[vertexNum].Data, posOffset);
			
			Vector2 tc1 = VectorMethods.FromArray2(tri[p1].Data, texOffset) - VectorMethods.FromArray2(tri[vertexNum].Data, texOffset);
			Vector2 tc2 = VectorMethods.FromArray2(tri[p2].Data, texOffset) - VectorMethods.FromArray2(tri[vertexNum].Data, texOffset);
			
			Vector3 tangent = (tc2.Y * vec1) - (tc1.Y * vec2);
			Vector3 bitangent = (-tc2.X * vec1) + (tc1.X * vec2);
			
			float[] tangents = new float[6];
			
			tangent.ToArray().CopyTo(tangents, 0, 0);
			bitangent.ToArray().CopyTo(tangents, 0, 3);
			
			return new float[6];
			
		}
		
		private void CreateArrays(bool generateTangents)
		{
			if (generateTangents && _vertexDeclaration.ContainsChannel("TEXCOORD"))
			{
				Console.WriteLine("generating tangent frames....");
				List<VertexChannel> originalChannels = new List<VertexChannel>();
				foreach (VertexChannel channel in _vertexDeclaration.channels)
				{
					originalChannels.Add(channel);
				}
				originalChannels.Add(new VertexChannel("TANGENT", _vertexDeclaration.Stride, 3));
				originalChannels.Add(new VertexChannel("BITANGENT", _vertexDeclaration.Stride + 3, 3));
				_vertexDeclaration = new VertexDeclaration(originalChannels);
				vertexData = new float[_vertexDeclaration.Stride * tris.Length * 3];
				indexData = new ushort[tris.Length * 3];
				int offset = 0;
				for (int i = 0; i < tris.Length; ++i)
				{
					for (int j = 0; j < 3; ++j)
					{
						tris[i][j].ToArray().CopyTo(vertexData, offset * _vertexDeclaration.Stride);
						indexData[i * 3 + j] = (ushort)offset;
						int tangentOffset = _vertexDeclaration.GetChannel("TANGENT").Offset;
						GenerateTangents(tris[i], j).CopyTo(vertexData, (offset * _vertexDeclaration.Stride) + tangentOffset);
						++offset;
					}
				}
			}
			else
			{
				vertexData = new float[_vertexDeclaration.Stride * tris.Length * 3];			
				indexData = new ushort[tris.Length * 3];
				int offset = 0;
				
				for (int i = 0; i < tris.Length; ++i)
				{
					for (int j = 0; j < 3; ++j)
					{
						tris[i][j].ToArray().CopyTo(vertexData, offset * _vertexDeclaration.Stride);
						indexData[i * 3 + j] = (ushort)offset;
						++offset;
					}
				}
			}
		}
		
		public static void PopulateArray (float[] source, int sourceOffset, float[] dest, int destOffset, int num)
		{
			for (int i = 0; i < num; i++)
			{
				dest[i + destOffset] = source[i + sourceOffset];  
			}
		}		
		
		private void PopulateMesh()
		{
			int offset = 0;
			
			tris = new Triangle[triCount];
			int triPos = 0;	
			
			for (int i = 0; i < polyVertexCount.Length; ++i)
			{
				IPoly poly;
			
				if (polyVertexCount[i] == 3)						
					poly = new Triangle();
				else						
					poly = new Quad();
				
				for (int j = 0; j < polyVertexCount[i]; ++j)
				{						
					float[] datArray = new float[_vertexDeclaration.Stride];						
					int index = 0;
				
					foreach (KeyValuePair<string, Semantic> inputSemantic in semantics)
					{
						var channel = _vertexDeclaration.GetChannel(inputSemantic.Key);
						index = indices[(semantics.Count * offset) + inputSemantic.Value.Offset];
						float[] sourceData = sources[inputSemantic.Value.SourceName].data;
						int sourceStride = sources[inputSemantic.Value.SourceName].stride;
						PopulateArray(sourceData, index * sourceStride, datArray, channel.Offset, channel.Stride);
					}							
				
					poly[j] = new Vertex(_vertexDeclaration);	
					poly[j].FromArray(datArray);
					++offset;							
				}
				if (polyVertexCount[i] == 4)
				{
					Triangle[] quadTris = PrimitiveHelper.ToTriangles(poly as Quad);
					quadTris.CopyTo(tris, triPos);						
					triPos += 2;
				}
				else
				{
					tris[triPos] = (Triangle)poly;
					++triPos;
				}
			}
			
			
		}
		
		private void ParseSource(XPathNavigator nav)
		{
			string sourceName = nav.GetAttribute("id", nav.GetNamespace("c"));
			Console.WriteLine("found source: " + sourceName);
			var arrayNode = nav.SelectSingleNode("c:float_array", nsManager);
			int arrayCount;
			if (!int.TryParse(arrayNode.GetAttribute("count", arrayNode.GetNamespace("c")), out arrayCount))
				throw new GeometryParserException("could not parse source: " + sourceName + " array");
			sources.Add(sourceName, new Source(new float[arrayCount]));
			string[] parts = arrayNode.InnerXml.Split(' ');
			
			for (int i = 0; i < arrayCount; ++i)
			{
				float fVal;
				if (!float.TryParse(parts[i], out fVal))
					throw new GeometryParserException("could not parse source: " + sourceName + " array, element: " + i);	
				sources[sourceName][i] = fVal;	    
			}
			
			var accessorNode = nav.SelectSingleNode("c:technique_common/c:accessor", nsManager);			
			int.TryParse(accessorNode.GetAttribute("stride", nsName), out sources[sourceName].stride);
		}		
		private void ParseMesh(XPathNavigator nav)
		{			
			var sourceIterator = nav.Select("c:source", nsManager);
			while (sourceIterator.MoveNext())
			{
				ParseSource(sourceIterator.Current);
			}
			
			var vertices = nav.SelectSingleNode("c:vertices", nsManager);
			string sourceNewName = vertices.GetAttribute("id", nsName);
			var input = nav.SelectSingleNode("c:vertices/c:input", nsManager);
			string sourceOldName = input.GetAttribute("source", nsName);
			sourceOldName = sourceOldName.Substring(1);
			
			float[] tmpData = sources[sourceOldName].data;
			int tmpStride = sources[sourceOldName].stride;
			
			sources.Remove(sourceOldName);
			sources.Add(sourceNewName, new Source(tmpData));
			sources[sourceNewName].stride = tmpStride;
			
			var polylist = nav.SelectSingleNode("c:polylist", nsManager);
			int polyCount;
			if (!int.TryParse(polylist.GetAttribute("count", nsName), out polyCount))
				throw new GeometryParserException("could not parse poly count!");
			
			polyVertexCount = new int[polyCount];
			
			var vcount = polylist.SelectSingleNode("c:vcount", nsManager);
			string[] parts = vcount.InnerXml.Split(' ');
			
			for (int i = 0; i < polyCount; ++i)
			{
				if (!int.TryParse(parts[i], out polyVertexCount[i]))
					throw new GeometryParserException("could not parse poly vertex count at element: " + i);
				
				if (polyVertexCount[i] == 3)
					++triCount;
				else if (polyVertexCount[i] == 4)
					triCount += 2;
				else
					throw new GeometryParserException("can only parse tris and quads!");
			}
			
			var inputIterator = polylist.Select("c:input", nsManager);
			int inputCount = 0;
			while (inputIterator.MoveNext())
			{
				string inputSemantic = inputIterator.Current.GetAttribute("semantic", nsName);
				string inputSource = inputIterator.Current.GetAttribute("source", nsName).Substring(1);
				int inputOffset;
				if (!int.TryParse(inputIterator.Current.GetAttribute("offset", nsName), out inputOffset))
					throw new GeometryParserException("could not parse input offset, (semantic " + inputSemantic + ")");
				semantics.Add(inputSemantic, new Semantic(inputSource, inputOffset));
				++inputCount;
			}		
			
			var indicesNode = polylist.SelectSingleNode("c:p", nsManager);
			parts = indicesNode.InnerXml.Split(' ');
			
			int indexCount = 0;
			
			foreach (int v in polyVertexCount)			
				indexCount += v;
			
			indexCount *= inputCount;
			indices = new int[indexCount];
			
			for (int i = 0; i < indexCount; ++i)
			{
				int.TryParse(parts[i], out indices[i]);
			}
			
						
			_vertexDeclaration = new VertexDeclaration(GetChannels());
			PopulateMesh();		
			
			Console.WriteLine("Finished Parsing Mesh");
		}
		
		
		public static int[] PolyToTris (int[] indices, int numberOfSides, int polyCount)
		{
			if (indices.Length != numberOfSides * polyCount)
				throw new ArgumentException("No. of indices must match the number of sides by the number of polys!");
			int trisPerPoly = (numberOfSides - 2);
			int triCount = trisPerPoly * polyCount;			
			int[] triIndices = new int[3 * triCount];
			int counter = 0;
			for (int i = 0; i < polyCount; ++i)
			{		
				int indexOffset = i * numberOfSides;
				for (int j = 0; j < (numberOfSides - 2); j++)
				{
					triIndices[counter + (j * 3)] = indices[indexOffset];
					triIndices[counter + 1 + (j * 3)] = indices[indexOffset + 1 + j];
					triIndices[counter + 2 + (j * 3)] = indices[indexOffset + 2 + j];
				}
				counter += (trisPerPoly * 3);
			}	
			return triIndices;
		}
		List<VertexChannel> GetChannels()			
		{
			List<VertexChannel> channels = new List<VertexChannel>();
			List<Semantic> l_semantics = new List<Semantic>();
			List<string> semanticNames = new List<string>();
			
			int lowest = int.MaxValue;
			
			Dictionary<string, Semantic> l_semDic = new Dictionary<string, Semantic>(semantics);
			
			KeyValuePair<string, Semantic> lowestKvp = new KeyValuePair<string, Semantic>("", new Semantic("", 0));
			
			while (l_semDic.Count != 0)
			{				
				foreach (KeyValuePair<string, Semantic> kvp in l_semDic)
				{
					if (kvp.Value.Offset < lowest)
					{
						lowest = kvp.Value.Offset;
						lowestKvp = kvp;
					}
				}
				l_semantics.Add(lowestKvp.Value);
				semanticNames.Add(lowestKvp.Key);
				l_semDic.Remove(lowestKvp.Key);
				lowest = int.MaxValue;
			}
			
			int counter = 0;
			
			for (int i = 0; i < l_semantics.Count; ++i)
			{
				int stride = sources[l_semantics[i].SourceName].stride;				                    
				channels.Add(new VertexChannel(semanticNames[i], counter, stride));
				counter += stride;
			}
			return channels;	
		}
				             
		public MeshElement Parse(XPathNavigator nav, string geomName, bool generateTangents = false)
		{
			
			Name = geomName;
			nsName = nav.GetNamespace("c");
			
			var mesh = nav.SelectSingleNode("c:mesh", nsManager);
			if (mesh == null)
				throw new GeometryParserException("Geometry does not contain mesh!");
			
			ParseMesh(mesh);
			
			CreateArrays(generateTangents);
			
			MeshElement retMesh = new MeshElement(_vertexDeclaration, vertexData, indexData);
			return retMesh;
			
		}
	}
}
