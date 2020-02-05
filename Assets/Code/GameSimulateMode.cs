using UnityEngine;
using System.Collections;
using System;

public class GameSimulateMode : MonoBehaviour
{
	public GameLevel level
	{
		get
		{
			return AppMain.inst.level;
		}
	}

	GameObject ballObj;
	public GameObject GetBall()
	{
		return ballObj;
	}

	private void OnEnable()
	{
		//Create ball
		ballObj = level.EjectBall();

		//Move camera
		var camera = Camera.main.GetComponent<CameraManager>();
		camera.targetObject = ballObj;
		camera.distance = 15.0f;
	}
	private void OnDisable()
	{
		GameObject.Destroy(ballObj);
	}
}
