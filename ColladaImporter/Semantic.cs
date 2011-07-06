using System;
using ModelMesh;

namespace ColladaImporter
{
	public class Semantic
	{
		public int Offset
		{
			get
			{
				return _offset;
			}
		}		
		public string SourceName
		{
			get 
			{
				return _sourceName;
			}
		}
		int _offset;
		string _sourceName;
		public Semantic(string pSourceName, int pOffset)
		{
			_offset = pOffset;
			_sourceName = pSourceName;
		}
	}
}
