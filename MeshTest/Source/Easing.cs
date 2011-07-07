using System;
namespace tkglengine
{
	public class Easing
	{
		public static void Logarithmic(ref float original, float target, float factor)
		{
			float diff = target - original;
			if (Math.Abs(diff) < 0.1f)
			{
				original += (diff / 2.0f);
			}
			if (Math.Abs(diff) < 0.00001f)
			{
				original = target;
				return;
			}
			diff *= factor;
			if (diff < 0.0f)
			{
				diff *= -1.0f;
				float change = ((float)Math.Log((double)diff + 1.0d) * diff);
				original -= change;
			}
			else
			{
				float change = ((float)Math.Log((double)diff + 1.0d) * diff);
				original += change;
			}
		}
		public Easing ()
		{
			
		}
	}
}

