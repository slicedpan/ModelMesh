using System;

namespace ColladaImporter
{
	public class Source
	{
		public float[] data;
		public int stride;
		public Source (float[] pData)			
		{
			data = pData;
		}
		public float this[int i]
		{
			get {return data[i];}
			set {data[i] = value;}
		}
	}
}
