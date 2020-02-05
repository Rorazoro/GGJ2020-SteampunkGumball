using UnityEngine;
using System;
using System.Collections.Generic;

public class GameEditMode : MonoBehaviour
{
	public GameLevel level
	{
		get
		{
			return AppMain.inst.level;
		}
	}
	public GameObject editorHud;

	public void OnEnable()
	{
		//Open tile menu
		editorHud.SetActive(true);

		//Move camera torwards the ball
		var camera = Camera.main.GetComponent<CameraManager>();
		camera.targetObject = null;
		camera.targetPosition = level.GetBallStartPos();
		camera.distance = distanceMax;
		camera.SnapTo();

		AppMain.inst.gridBackground.SetActive(true);
	}
	public void OnDisable()
	{
		editorHud.SetActive(false);
		AppMain.inst.gridBackground.SetActive(false);
	}
	public void OnMouseOver()
	{
		
	}

	public void Start()
	{
		
	}
	public void Update()
	{
		UpdateCamera();
	}

	float distanceMin = 15f;
	float distanceMax = 50f;
	void UpdateCamera()
	{
		var camera = Camera.main.GetComponent<CameraManager>();

		{
			//WASD Movement
			var moveDir = Vector3.zero;
			if (Input.GetKey(KeyCode.A))
				moveDir += Vector3.left;
			if (Input.GetKey(KeyCode.D))
				moveDir += Vector3.right;
			if (Input.GetKey(KeyCode.W))
				moveDir += Vector3.up;
			if (Input.GetKey(KeyCode.S))
				moveDir += Vector3.down;
			moveDir = moveDir.normalized;

			float cameraMoveSpeed = camera.distance;
			camera.targetObject = null;
			camera.targetPosition += moveDir * cameraMoveSpeed * Time.deltaTime;
		}

		//Mouse offsets
		/*{
			float margin = 128.0f;
			float cameraMoveSpeed = 0.1f;

			var moveDir = Vector3.zero;
			var mousePos = Input.mousePosition;
			if (mousePos.x < margin)
			{
				moveDir = Vector3.left * (margin - mousePos.x);
			}
			if (mousePos.y < margin)
			{
				moveDir = Vector3.down * (margin - mousePos.y);
			}
			if (mousePos.x > Camera.main.pixelWidth-margin)
			{
				moveDir = Vector3.right * (mousePos.x - (Camera.main.pixelWidth - margin));
			}
			if (mousePos.y > Camera.main.pixelHeight - margin)
			{
				moveDir = Vector3.up * (mousePos.y - (Camera.main.pixelHeight - margin));
			}

			camera.targetObject = null;
			camera.targetPosition += moveDir * cameraMoveSpeed * Time.deltaTime;
		}*/

		//Return to center
		if(Input.GetKeyDown(KeyCode.F))
		{
			camera.targetPosition = level.GetBallStartPos();
			camera.distance = distanceMax;
		}

		//Mouse Drag Movement
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			//Record current mouse pos
			mouseDownOffset = Input.mousePosition;
			mouseDownCenter = AppMain.MouseToGrid(new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f, 0));
			mouseDownPos = AppMain.MouseToGrid(Input.mousePosition);
		}
		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			var diff = mouseDownOffset-Input.mousePosition;
			camera.targetPosition = mouseDownCenter + (diff * camera.distance / 800.0f);
		}

		//Zoom
		float scrollSpeed = 2.0f;
		camera.distance += -Input.mouseScrollDelta.y * scrollSpeed;
		camera.distance = Mathf.Clamp(camera.distance, distanceMin, distanceMax);
	}

	Vector3 mouseDownOffset;
	Vector3 mouseDownCenter;
	Vector3 mouseDownPos;
}