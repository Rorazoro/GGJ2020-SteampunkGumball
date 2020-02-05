using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
	public GameObject spawnPoint;
	public GameObject fireworks;

	float timer = 0;
	float lifetime = 2.0f;
	GameObject ball;

	public void OnTriggerEnter(Collider collider)
	{
		if(ball == null)
		{
			if(collider.gameObject == AppMain.inst.simulateMode.GetBall())
			{
				this.ball = collider.gameObject;
			}
		}
	}

	public void Update()
	{
		if(ball != null)
		{
			//Move towards us
			var rigidBody = ball.GetComponent<Rigidbody>();
			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = false;
			rigidBody.AddForce((transform.position - rigidBody.position) * 50.0f, ForceMode.Acceleration);

			//Check timer
			timer += Time.deltaTime;
			if(timer >= lifetime)
			{
				//Move to spawn point
				ball.transform.position = spawnPoint.transform.position;
				rigidBody.velocity = Vector3.zero;
				rigidBody.useGravity = true;
				rigidBody.WakeUp();
				ball = null;

				//Win
				AppMain.inst.TriggerWin();

				//Fireworks
				fireworks.SetActive(true);
			}
		}
	}


}
