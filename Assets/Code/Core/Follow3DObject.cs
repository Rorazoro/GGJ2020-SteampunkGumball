using UnityEngine;
using System.Collections;

public class Follow3DObject : MonoBehaviour
{
	public GameObject target;
	public Vector2 screenSpaceOffset;
	void Start()
	{
		updatePosition();
	}
	void Update()
	{
		updatePosition();
	}
	void updatePosition()
	{
		var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
		transform.position = screenPos + (Vector3)screenSpaceOffset;
	}
}
