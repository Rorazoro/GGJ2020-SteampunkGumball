using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHelper : MonoBehaviour
{
	float radius;
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PerformJump();
		}
    }
	void PerformJump()
	{
		var collider = GetComponentInChildren<Collider>();
		var results = Physics.OverlapSphere(transform.position, 0.6f);
		foreach(var result in results)
		{
			if (result == collider)
				continue;

			//Jump
			var rigidBody = GetComponent<Rigidbody>();
			var force = Quaternion.AngleAxis(Random.Range(-10f, 10f), Vector3.back) * Vector3.up * 3.0f;
			rigidBody.AddForce(force, ForceMode.Impulse);
			return;
		}
	}
}
