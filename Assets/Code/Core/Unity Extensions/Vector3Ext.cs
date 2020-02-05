using UnityEngine;
using System.Collections;

public static class Vector3Ext
{
	public static Vector3 Floor(Vector3 value)
	{
		return new Vector3(Mathf.Floor(value.x), Mathf.Floor(value.y), Mathf.Floor(value.z));
	}
	public static Vector3 Ceil(Vector3 value)
	{
		return new Vector3(Mathf.Ceil(value.x), Mathf.Ceil(value.y), Mathf.Ceil(value.z));
	}
	public static Vector3 Divide(Vector3 valueA, Vector3 valueB)
	{
		return new Vector3(valueA.x / valueB.x, valueA.y / valueB.y, valueA.z / valueB.z);
	}
}
