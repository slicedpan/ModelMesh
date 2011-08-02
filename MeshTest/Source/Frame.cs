using System;
namespace MeshTest
{
	public struct Frame
	{
		public int xOffset, yOffset, width, height;
		public Frame (int x, int y, int w, int h)
		{
			xOffset = x;
			yOffset = y;
			width = w;
			height = h;
		}
	}
}

