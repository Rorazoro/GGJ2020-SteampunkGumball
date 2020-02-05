using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : MonoBehaviour
{
	int ballsToGenerate = 25;
	List<GameObject> balls = new List<GameObject>();
	public GameObject ballPrefab;
	public GameObject generatePoint;
	public GameObject spawnPoint;

	public List<Material> materials = new List<Material>();

	private void Start()
	{
		//Generate a bunch of balls
		for(int i=0; i< ballsToGenerate; i++)
		{
			GenerateBall();
		}
	}
	void GenerateBall()
	{
		Vector3 spawnPoint = generatePoint.transform.position;
		float spawnRadius = 0.5f;
		float spawnHeight = 0.4f;

		var obj = GameObject.Instantiate(ballPrefab);
		obj.transform.position = spawnPoint + (Vector3.up * Random.Range(-spawnHeight, spawnHeight)) + (Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Vector3.forward * Random.Range(0, spawnRadius));
		balls.Add(obj);
		AppMain.inst.level.gameObject.AddChild(obj);

		var rigidBody = obj.GetComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.None;

		obj.GetComponent<MeshRenderer>().material = materials.Random();

		//Disable trail
		obj.GetComponentInChildren<TrailRenderer>().enabled = false;
	}

	public GameObject EjectBall()
	{
		//Find ball with lowest position
		float lowestDistance = float.MaxValue;
		GameObject obj = null;
		foreach(var ball in balls)
		{
			if(ball.transform.position.y < lowestDistance)
			{
				obj = ball;
				lowestDistance = ball.transform.position.y;
			}
		}

		//Move
		obj.transform.position = spawnPoint.transform.position;
		var rigidBody = obj.GetComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
		rigidBody.velocity = Vector3.zero;

		//Enable trail
		obj.GetComponentInChildren<TrailRenderer>().enabled = true;

		//Generate a replacement
		balls.Remove(obj);
		GenerateBall();

		//Return
		return obj;
	}
}
