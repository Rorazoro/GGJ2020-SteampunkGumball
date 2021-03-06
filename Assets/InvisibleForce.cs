﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleForce : MonoBehaviour
{
	public Vector3 baseDirection = Vector3.up;
	public float force = 10.0f;
	public GameObject parent;

	public void OnTriggerStay(Collider other)
	{
		var rigidBody = other.GetComponent<Rigidbody>();
		if(rigidBody != null)
		{
			var rotation = parent != null ? parent.transform.rotation : this.transform.rotation;
			rigidBody.AddForce((rotation * baseDirection) * force, ForceMode.Acceleration);
		}
	}
}
