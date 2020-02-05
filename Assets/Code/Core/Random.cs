using UnityEngine;
using System.Collections;

namespace Game
{
	public static class Random
	{

		public static int Range(int min, int max)
		{
			return UnityEngine.Random.Range(min, max + 1);
		}
		public static float Range(float min, float max)
		{
			return UnityEngine.Random.Range(min, max);
		}
	}


}
