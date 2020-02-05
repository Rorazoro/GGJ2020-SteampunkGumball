using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
	[NonSerialized] public GameObject targetObject;
	[NonSerialized] public Vector3 targetPosition;
	float speed = 5.0f;
	public float distance = 10.0f;
	public float bounds = 100.0f;

	public void Update()
	{
		//Move towards target
		var target = CalcTarget();
		target = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
		transform.position = target;
	}
	public void SnapTo()
	{
		transform.position = CalcTarget();
	}
	public Vector3 CalcTarget()
	{
		//Update origin
		if (targetObject != null)
		{
			targetPosition = targetObject.transform.position;
		}
		targetPosition.x = Mathf.Clamp(targetPosition.x, -bounds, bounds);
		targetPosition.y = Mathf.Clamp(targetPosition.y, -bounds, bounds);
		targetPosition.z = 0;

		//Move towards target
		var actualTarget = targetPosition + (Vector3.back * distance);

		//Return
		return actualTarget;
	}
}