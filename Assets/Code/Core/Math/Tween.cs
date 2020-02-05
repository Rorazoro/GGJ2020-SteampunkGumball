using System;

public static class Tween
{
	public delegate float TweenMethod(float time);

	//Linear
	public static float Linear(float time)
	{
		return time;
	}

	//Quadratic
	public static float QuadraticEaseIn(float time)
	{
		return time * time;
	}
	public static float QuadraticEaseOut(float time)
	{
		return -time * (time - 2.0f);
	}
	public static float QuadraticEaseInOut(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (time * time) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			return 0.5f + (-time * (time - 2.0f)) * 0.5f;
		}
	}
	public static float QuadraticEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (-time * (time - 2.0f)) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			return 0.5f + (time * time) * 0.5f;
		}
	}
	public static float QuadraticBezier(float time, float control)
	{
		return 1.0f - (control * (2.0f * time * (1.0f - time)) + ((1.0f - time) * (1.0f - time)));
	}

	//Cubic
	public static float CubicEaseIn(float time)
	{
		return time * time * time;
	}
	public static float CubicEaseOut(float time)
	{
		time = time - 1.0f;
		return (time * time * time) + 1.0f;
	}
	public static float CubicEaseInOut(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (time * time * time) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			time = time - 1.0f;
			return 0.5f + (((time * time * time) + 1.0f) * 0.5f);
		}
	}
	public static float CubicEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			time = time - 1.0f;
			return ((time * time * time) + 1.0f) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			return 0.5f + ((time * time * time) * 0.5f);
		}
	}

	//Quartic
	public static float QuarticEaseIn(float time)
	{
		return time * time * time * time;
	}
	public static float QuarticEaseOut(float time)
	{
		time = time - 1.0f;
		return -(time * time * time * time - 1.0f);
	}
	public static float QuarticEaseInOut(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (time * time * time * time) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			time = time - 1.0f;
			return 0.5f + (-(time * time * time * time - 1.0f)) * 0.5f;
		}
	}
	public static float QuarticEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			time = time - 1.0f;
			return (-(time * time * time * time) + 1.0f) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			return 0.5f + ((time * time * time * time) * 0.5f);
		}
	}

	//Quintic
	public static float QuinticEaseIn(float time)
	{
		return time * time * time * time * time;
	}
	public static float QuinticEaseOut(float time)
	{
		time = time - 1.0f;
		return time * time * time * time * time + 1.0f;
	}
	public static float QuinticEaseInOut(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (time * time * time * time * time) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			time = time - 1.0f;
			return 0.5f + (time * time * time * time * time + 1.0f) * 0.5f;
		}
	}
	public static float QuinticEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			time = time - 1.0f;
			return ((time * time * time * time * time) + 1.0f) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			return 0.5f + ((time * time * time * time * time) * 0.5f);
		}
	}

	//Sine
	public static float SineEaseIn(float time)
	{
		return 1.0f - (float)System.Math.Cos(time * Numerics.HALF_PI);
	}
	public static float SineEaseOut(float time)
	{
		return (float)System.Math.Sin(time * Numerics.HALF_PI);
	}
	public static float SineEaseInOut(float time)
	{
		return (1.0f - (float)System.Math.Cos(time * Numerics.PI)) * 0.5f;
	}
	public static float SineEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (float)System.Math.Sin(time * Numerics.HALF_PI) * 0.5f;
		}
		else
		{
			time *= 2.0f;
			time = time - 1.0f;
			return 1.0f - (float)System.Math.Cos(time * Numerics.HALF_PI) * 0.5f;
		}
	}

	//Exponential
	public static float ExponentialEaseIn(float time)
	{
		return (float)System.Math.Pow(2.0f, 10.0f * (time - 1.0f));
	}
	public static float ExponentialEaseOut(float time)
	{
		return -(float)System.Math.Pow(2.0f, -10.0f * time) + 1.0f;
	}
	public static float ExponentialEaseInOut(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (float)System.Math.Pow(2.0f, 10.0f * (time - 1.0f)) * 0.5f;
		}
		else
		{
			time *= 2.0f;
			time = time - 1.0f;
			return 1.0f + (-(float)System.Math.Pow(2.0f, -10.0f * time)) * 0.5f;
		}
	}
	public static float ExponentialEaseOutIn(float time)
	{
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (-(float)System.Math.Pow(2.0f, -10.0f * time)) * 0.5f + 0.5f;
		}
		else
		{
			time *= 2.0f;
			time = time - 1.0f;
			return 0.5f + (float)System.Math.Pow(2.0f, 10.0f * (time - 1.0f)) * 0.5f;
		}
	}

	//Circular
	public static float CircularEaseIn(float time)
	{
		if(time < 0) time = 0;
		if(time > 1) time = 1;
		return 1.0f - (float)System.Math.Sqrt(1.0f - (time * time));
	}
	public static float CircularEaseOut(float time)
	{
		if(time < 0) time = 0;
		if(time > 1) time = 1;
		time = time - 1.0f;
		return (float)System.Math.Sqrt(1.0f - (time * time));
	}
	public static float CircularEaseInOut(float time)
	{
		if(time < 0) time = 0;
		if(time > 1) time = 1;
		if(time < 0.5f)
		{
			time *= 2.0f;
			return (1.0f - (float)System.Math.Sqrt(1.0f - (time * time))) * 0.5f;
		}
		else
		{
			time = (time - 0.5f) * 2.0f;
			time = time - 1.0f;
			return 0.5f + ((float)System.Math.Sqrt(1.0f - (time * time))) * 0.5f;
		}
	}
	public static float CircularEaseOutIn(float time)
	{
		if(time < 0) time = 0;
		if(time > 1) time = 1;
		if(time < 0.5f)
		{
			time *= 2.0f;
			time = time - 1.0f;
			return (float)System.Math.Sqrt(1.0f - (time * time)) * 0.5f;
		}
		else
		{
			time *= 2.0f;
			time = time - 1.0f;
			return 1.0f - (float)System.Math.Sqrt(1.0f - (time * time)) * 0.5f;
		}
	}

	//Bounce
	public static float BounceEaseIn(float time)
	{
		return 1.0f - BounceEaseOut(1.0f - time);
	}
	public static float BounceEaseOut(float time)
	{
		if(time < (1 / 2.75f))
		{
			return (7.5625f * time * time);
		}
		else if(time < (2 / 2.75f))
		{
			float postFix = time -= (1.5f / 2.75f);
			return (7.5625f * (postFix) * time + .75f);
		}
		else if(time < (2.5 / 2.75))
		{
			float postFix = time -= (2.25f / 2.75f);
			return (7.5625f * (postFix) * time + .9375f);
		}
		else
		{
			float postFix = time -= (2.625f / 2.75f);
			return (7.5625f * (postFix) * time + .984375f);
		}
	}
	public static float BounceEaseInOut(float time)
	{
		if(time < 0.5f)
			return BounceEaseIn(time * 2.0f) * 0.5f;
		else
			return 0.5f + BounceEaseOut((time - 0.5f) * 2.0f) * 0.5f;
	}

	//Back
	public static float BackEaseIn(float time)
	{
		float s = 1.70158f;
		return (time * time * ((s + 1.0f) * time - s));
	}
	public static float BackEaseOut(float time)
	{
		float s = 1.70158f;
		return ((time = time - 1.0f) * time * ((s + 1.0f) * time + s) + 1.0f);
	}

	//Elastic
	public static float ElasticEaseIn(float time)
	{
		return 1.0f - ElasticEaseOut(1.0f - time);
	}
	public static float ElasticEaseOut(float time)
	{
		//Bounds
		if(time <= 0.0f)
			return 0;
		if(time >= 1.0f)
			return 1;

		float a = 0.1f;  //Amplitude
		float p = 0.75f; //Period
		float s;
		if(a == 0.0f || a < 1)
		{
			a = 1;
			s = p / 4;
		}
		else
			s = p / (2 * Numerics.PI) * (float)System.Math.Asin(1 / a);
		return (a * (float)System.Math.Pow(2, -10 * time) * (float)System.Math.Sin((time - s) * (2 * Numerics.PI) / p) + 1);
	}

	//Interpolate

	//From time 0-1 we tween from 0-1-0 using the tween method on both ends
	public static float InterpolateInOut(float time, Tween.TweenMethod tween)
	{
		if(time < 0.5f)
		{
			return tween(time / 0.5f);
		}
		else
		{
			return tween(1.0f - ((time - 0.5f) / 0.5f));
		}
	}
	public static float InterpolateInOut(float time, Tween.TweenMethod tweenIn, Tween.TweenMethod tweenOut)
	{
		if(time < 0.5f)
		{
			return tweenIn(time / 0.5f);
		}
		else
		{
			return 1.0f-tweenOut((time - 0.5f) / 0.5f);
		}
	}
	/*public static float InterpolateInPauseOut(float time, float pauseRatio, Tween.TweenMethod tweenIn, Tween.TweenMethod tweenOut)
	{
		float inRatio = (1.0f - pauseRatio) * 0.5f;
		//float outRatio = 
		if(time < inRatio)
		{
			return tweenIn((time / inRatio));
		}
		else if(time < inRatio+pauseRatio)
		{
			return 

		}

		if(time < 0.5f)
		{
			return tweenIn(time / 0.5f);
		}
		else
		{
			return 1.0f - tweenOut((time - 0.5f) / 0.5f);
		}
	}*/
};