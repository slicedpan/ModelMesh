using System;
using System.Collections.Generic;

namespace MeshTest
{
	public class Animator
	{		
		Dictionary<string, Animation> animations;
		string currentAnimation;
		double time = 0;
		public Animator ()
		{
			animations = new Dictionary<string, Animation>();
			currentAnimation = "";
		}
		public void Update(double timeElapsed)
		{
			time += timeElapsed;
			if (time > (1.0d / animations[currentAnimation].speed))
			{
				animations[currentAnimation].Step();
				time = 0;
			}
		}
		public void AddAnimation(string name, Animation animation)
		{
			animations.Add(name, animation);
			if (currentAnimation == "")
				currentAnimation = name;
		}
		public Frame Current
		{
			get
			{
				return animations[currentAnimation].Current;
			}
		}
		public void Select(string name)
		{
			if (!animations.ContainsKey(name))
				throw new Exception("Animator does not contain an animation called " + name);
			currentAnimation = name;
			animations[currentAnimation].Reset();
		}
	}
}

