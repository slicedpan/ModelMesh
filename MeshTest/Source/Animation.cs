using System;
using System.Collections.Generic;

namespace MeshTest
{
	public class Animation
	{
		int maxFrames = 0;
		int currentFrame = 0;
		public double speed = 15;
		List<Frame> frames;
		public Frame Current
		{
			get 
			{
				return frames[currentFrame];
			}
		}
		public Animation ()
		{
			frames = new List<Frame>();
		}
		public void Step()
		{
			currentFrame++;
			if (currentFrame >= maxFrames)
				currentFrame = 0;
		}
		public void Add(int x, int y, int w, int h)
		{
			maxFrames++;
			frames.Add(new Frame(x, y, w, h));
		}
		public void Reset()
		{
			currentFrame = 0;
		}
	}
}

