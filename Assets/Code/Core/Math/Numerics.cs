using System;

public static class Numerics
{
	public static float PI = 3.14159265358979338f;
	public static float TWO_PI = (PI * 2);
	public static float HALF_PI = 1.570796326796f;
	public static float QUARTER_PI = 0.785398f;

	public static float Modulus(float value, float reference)
	{
		//Calculate the modulus
		value -= reference * ((int)(value / reference));
		if(value < 0.0f)
			value = System.Math.Max(value, -reference);
		else
			value = System.Math.Min(value, reference);

		//Return result
		return value;
	}
	public static float Clamp(float value, float min, float max)
	{
		if(value < min)
			return min;
		if(value > max)
			return max;
		return value;
	}
	public static float Lerp(float source, float dest, float value)
	{
		//Clamp
		if(value < 0.0f)
			value = 0.0f;
		else if(value > 1.0f)
			value = 1.0f;

		//Calculate
		return (source * (1.0f - value)) + (dest * value);
	}
	public static float FloorToRef(float source, float reference)
	{
		return (float)System.Math.Floor(source / reference) * reference;
	}
	public static int Cyclic(int value, int reference) //Return 0 to reference.  Similar to modulo except negatives are wrapped around to the end of the range.
	{
		value = value % reference;
		if(value < 0)
			value = reference + value;
		return value;
	}
}

