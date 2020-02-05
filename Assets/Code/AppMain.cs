using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AppMain : MonoBehaviour
{
	public static AppMain inst;

	[NonSerialized] public GameLevel level;
	[NonSerialized] public GameEditMode editMode;
	[NonSerialized] public GameSimulateMode simulateMode;
	public GameObject canvas;
	public UnityEngine.UI.Image screenFade;
	public GameObject gridBackground;
	public GameObject winMenu;
	public GameObject mainMenu;
	public GameObject playButton;

	public List<GameObject> levels = new List<GameObject>();

	public Game.ActionQueue actionQueue = new Game.ActionQueue();

	public List<GameObject> cleanupBeforeNextLevel = new List<GameObject>();
	public void Cleanup()
	{
		foreach(var item in cleanupBeforeNextLevel)
		{
			GameObject.Destroy(item);
		}
		cleanupBeforeNextLevel.Clear();
	}

	

    // Start is called before the first frame update
    void Start()
    {
		inst = this;

		//Modes
		editMode = GetComponent<GameEditMode>();
		simulateMode = GetComponent<GameSimulateMode>();

		//Load main menu
		ReturnToMainMenu();
	}
	private void Update()
	{
		actionQueue.update(Time.deltaTime);
	}

	/*public void LoadLevel(string name)
	{
		//Create
		var prefab = Resources.Load<GameObject>("Levels/" + name);
		if(prefab == null)
			Debug.LogError("Unable to load level:" + name);

		//Load
		LoadLevel(prefab);
	}*/

	public void ReturnToMainMenu()
	{
		//Cleanup
		Cleanup();
		playButton.SetActive(false);

		//Destroy level
		editMode.enabled = false;
		simulateMode.enabled = false;
		if(level != null)
			GameObject.Destroy(level.gameObject);

		//Create main menu
		var obj = GameObject.Instantiate(mainMenu);
		cleanupBeforeNextLevel.Add(obj);
		canvas.AddChild(obj);
		obj.transform.SetSiblingIndex(0);
	}

	public void LoadLevelRaw(GameObject prefab)
	{
		//Setup
		hasCompletedLevel = false;
		playButton.SetActive(true);

		//Cleanup
		if (level != null)
		{
			GameObject.Destroy(level.gameObject);
		}
		editMode.enabled = false;
		simulateMode.enabled = false;

		//Load
		var levelObj = GameObject.Instantiate(prefab);
		level = levelObj.GetComponent<GameLevel>();
		level.ourPrefab = prefab;
	}
	public void LoadLevel(GameObject levelPrefab)
	{
		//Fade out
		actionQueue.push(new FadeOut(1.0f));

		//Load
		actionQueue.push(new LoadLevel(levelPrefab));

		//Begin
		actionQueue.push(new BeginLevel());

		//Fade in
		actionQueue.push(new FadeIn(1.0f));
	}
	public void LoadNextLevel()
	{
		//Find level
		var levelIndex = levels.IndexOf(level.ourPrefab);
		levelIndex += 1;
		levelIndex = levelIndex % levels.Count;
		LoadLevel(levels[levelIndex]);
	}

	public void TogglePlayMode()
	{
		if (hasCompletedLevel)
			return;

		if(editMode.enabled)
		{
			editMode.enabled = false;
			simulateMode.enabled = true;
		}
		else
		{
			editMode.enabled = true;
			simulateMode.enabled = false;
		}
	}

	public bool hasCompletedLevel = false;
	public void TriggerWin()
	{
		if (hasCompletedLevel)
			return;
		hasCompletedLevel = true;

		//Open win menu
		var menu = GameObject.Instantiate(winMenu);
		canvas.AddChild(menu);
		menu.transform.SetSiblingIndex(0);
		cleanupBeforeNextLevel.Add(menu);

		//Continue
		{
			var button = menu.FindChild("Continue").GetComponent<SimpleButton>();
			button.eventOnClick.AddListener(LoadNextLevel);
		}
		
		{
			var button = menu.FindChild("Return").GetComponent<SimpleButton>();
			button.eventOnClick.AddListener(ReturnToMainMenu);
		}
	}

	public void Quit()
	{
		UnityEngine.Application.Quit();
	}

	public static Vector3 MouseToGrid(Vector3 mousePos)
	{
		var plane = new Plane(Vector3.back, 0);
		var ray = Camera.main.ScreenPointToRay(mousePos);
		float enter;
		plane.Raycast(ray, out enter);
		return ray.GetPoint(enter);
	}
	public static bool CheckIfMouseOverUI(Vector3 mousePos)
	{
		var raycaster = AppMain.inst.canvas.GetComponent<UnityEngine.UI.GraphicRaycaster>();
		var data = new PointerEventData(EventSystem.current);
		data.position = mousePos;
		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(data, results);
		if (results.Count > 0)
			return true;
		else
			return false;
	}
}
