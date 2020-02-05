using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct BoundingRect
{
	public Vector3 min;
	public Vector3 max;

	public Vector3 origin
	{
		get
		{
			return (min + max) * 0.5f;
		}
		set
		{
			var halfExtents = this.halfExtents;
			min = value - halfExtents;
			max = value + halfExtents;
		}
	}
	public Vector3 halfExtents
	{
		get
		{
			return (max - min) * 0.5f;
		}
	}

	public BoundingRect(Vector3 min, Vector3 max)
	{
		this.min = min;
		this.max = max;
	}
	public bool Contains(Vector3 value)
	{
		return (
			min.x >= value.x && min.y >= value.y && min.y >= value.y &&
			max.x <= value.x && max.y <= value.y && max.y <= value.y);
	}
	public bool Intersects(BoundingRect rect)
	{
		return (
			this.min.x <= rect.max.x && this.min.y <= rect.max.y && this.min.z <= rect.max.z &&
			this.max.x >= rect.min.x && this.max.y >= rect.min.y && this.max.z >= rect.min.z);

	}
	public static bool operator ==(BoundingRect rectA, BoundingRect rectB)
	{
		return (rectA.min == rectB.min) && (rectA.max == rectB.max);
	}
	public static bool operator !=(BoundingRect rectA, BoundingRect rectB)
	{
		return (rectA.min != rectB.min) || (rectA.max != rectB.max);
	}
}
