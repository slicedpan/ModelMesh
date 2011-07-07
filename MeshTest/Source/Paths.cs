using System;
using System.IO;

namespace tkglengine
{
	public class Paths
	{
		public static string ModelPath;
		public static string ShaderPath;		
		
		public static void Init()
		{
			string currentDir = Directory.GetCurrentDirectory();						
			string rootDir = Directory.GetParent(Directory.GetParent(currentDir).FullName).FullName;
			string[] subdirs = Directory.GetDirectories(rootDir);
			
			
			
			foreach (string path in subdirs)
			{
				if (TopLevel(path) == "Models")
					ModelPath = path + Path.DirectorySeparatorChar;
				else if (TopLevel(path) == "Shaders")
					ShaderPath = path + Path.DirectorySeparatorChar;
			}
		}
		public static string TopLevel(string path)
		{
			string[] parts = path.Split("\\/".ToCharArray());
			return parts[parts.Length - 1];
		}
		
	}
}

