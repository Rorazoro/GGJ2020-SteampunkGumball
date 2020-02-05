using UnityEngine;
using System.Collections;

namespace Game
{
	public class TransformState
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		public TransformState()
		{
			clear();
		}
		public void clear()
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.one;
		}
	}
}