using UnityEngine;
using System.Collections;

public static class CatmullRomSpline
{
	public struct Spline
	{
		public Vector3[] points;
		public bool closedLoop;

		public Vector3 evaluate(float t)
		{
			Vector3 result = Vector3.zero;
			int pointIndex;
			int size;
			float tempTimeA;
			float tempTimeB;

			if(t < 0.0f)
				t = 0.0f;
			else if(t > 1.0f)
				t = 1.0f;

			//Find which point should be evaluated
			if(closedLoop)
			{
				size = points.Length;
				if(size < 3)
					return result;

				pointIndex = (int)((float)size * t);

				//Find the correct time
				tempTimeA = (1.0f / (float)size) * ((float)pointIndex);
				tempTimeB = (1.0f / (float)size) * ((float)pointIndex + 1);
				t = (t - tempTimeA) / (tempTimeB - tempTimeA);
			}
			else
			{
				size = points.Length - 3;
				if(size < 1)
					return result;

				pointIndex = (int)((float)size * t);
				pointIndex += 2;
				if(t == 1.0f)
					pointIndex -= 1;

				//Find the correct time
				tempTimeA = (1.0f / (float)size) * ((float)pointIndex - 2);
				tempTimeB = (1.0f / (float)size) * ((float)pointIndex - 1);
				t = (t - tempTimeA) / (tempTimeB - tempTimeA);
			}

			return evaluate(pointIndex, t);
		}
		public Vector3 evaluate(int i, float t)
		{
			Vector3 result = Vector3.zero;
			int j;
			int size;
			int temp;

			//Figure out the result
			if(closedLoop)
			{
				size = points.Length;
				for(j = -2; j <= 1; j++)
				{
					temp = i + j;
					if(temp < 0)
						temp = size - (-temp % size);
					else
						temp = temp % size;
					result += points[temp] * basis(j, t);
				}
			}
			else
			{
				for(j = -2; j <= 1; j++)
					result += points[i + j] * basis(j, t);
			}


			return result;
		}
		float basis(int i, float t)
		{
			switch(i)
			{
				case -2:
					return ((-t + 2.0f) * t - 1) * t / 2.0f;
				case -1:
					return (((3.0f * t - 5.0f) * t) * t + 2.0f) / 2.0f;
				case 0:
					return ((-3.0f * t + 4.0f) * t + 1) * t / 2.0f;
				case 1:
					return ((t - 1.0f) * t * t) / 2.0f;
				default:
					return 0.0f;
			}
		}
	}

	public static Vector3 Evaluate(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}
}