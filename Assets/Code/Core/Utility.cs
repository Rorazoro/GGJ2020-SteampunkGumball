using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public static class Utility
	{
		//Vector2
		public static Vector2 Divide(Vector2 valueA, Vector2 valueB)
		{
			return new Vector2(valueA.x / valueB.x, valueA.y / valueB.y);
		}

		//Color
		public static UnityEngine.Color HSV(float hue, float saturation, float light)
		{
			var color = new UnityEngine.Color(hue, saturation, light, 1.0f);
			color = color.HSVtoRGB();
			return color;
		}

		public static UnityEngine.Color Hex(string hex)
		{
			Color color;
			if(UnityEngine.ColorUtility.TryParseHtmlString(hex, out color))
				return color;
			else
				return Color.white;
		}

		//Angle
		public static float AngleBetweenTwoUnitVectors(Vector3 planeNormal, Vector3 vectorA, Vector3 vectorB)
		{
			//Find the cross product
			var cross = Vector3.Cross(vectorA, planeNormal);
			if(Vector3.Dot(cross, vectorB) > 0.0f)
				return Numerics.TWO_PI - Mathf.Acos(Mathf.Clamp(Vector3.Dot(vectorA, vectorB), -1.0f, 1.0f));
			else
				return Mathf.Acos(Mathf.Clamp(Vector3.Dot(vectorA, vectorB), -1.0f, 1.0f));
		}
		public static float AngleClosestBetweenTwoUnitVectors(Vector3 planeNormal, Vector3 vectorA, Vector3 vectorB)
		{
			//Find the cross product
			Vector3 cross = Vector3.Cross(vectorA, planeNormal);
			if(Vector3.Dot(cross, vectorB) > 0.0f)
				return -Mathf.Acos(Mathf.Clamp(Vector3.Dot(vectorA, vectorB), -1.0f, 1.0f));
			else
				return Mathf.Acos(Mathf.Clamp(Vector3.Dot(vectorA, vectorB), -1.0f, 1.0f));
		}
		public static float SlerpAngle(float angleA, float angleB, float ratio)
		{
			var diff = angleB - angleA;
			if (diff > 180f)
				angleA += 360f;
			else if (diff < -180f)
				angleA -= 360f;
			return Mathf.Lerp(angleA, angleB, ratio);
		}

		//Rect
		public static Rect RectTransformToScreenSpace(RectTransform transform)
		{
			Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
			return new Rect((Vector2)transform.position - (size * 0.5f), size);
		}
		public static Rect KeepRectInsideRect(Rect inner, Rect outer)
		{
			Vector2 offset = Vector2.zero;
			if(inner.min.x < outer.min.x)
				offset.x += (outer.min.x - inner.min.x);
			if(inner.min.y < outer.min.y)
				offset.y += (outer.min.y - inner.min.y);
			if(inner.max.x > outer.max.x)
				offset.x -= (inner.max.x - outer.max.x);
			if(inner.max.y > outer.max.y)
				offset.y -= (inner.max.y - outer.max.y);

			inner.position += offset;
			return inner;
		}
		public static Rect CombineRect(Rect rectA, Rect rectB)
		{
			Rect result = new Rect();
			result.xMin = Mathf.Min(rectA.xMin, rectB.xMin);
			result.yMin = Mathf.Min(rectA.yMin, rectB.yMin);
			result.xMax = Mathf.Max(rectA.xMax, rectB.xMax);
			result.yMax = Mathf.Max(rectA.yMax, rectB.yMax);
			return result;
		}

		//List
		public static void Reserve<Type>(this List<Type> list, int amount)
		{
			if (list.Capacity < amount)
				list.Capacity = amount;
		}
	}
}