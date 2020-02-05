using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePalette : MonoBehaviour
{
	public GameObject buttonPrefab;
	public TMPro.TextMeshProUGUI tileCountText;

	public void OnEnable()
	{
		var level = AppMain.inst.level;

		//Create buttons
		gameObject.DestroyChildren();

		//Create destroy
		{
			var tile = Resources.Load<GameTile>("Tiles/Erase/Tile");

			var buttonObj = GameObject.Instantiate(buttonPrefab);
			var buttonScript = buttonObj.GetComponent<TileButton>();
			buttonScript.SetTile(tile);
			buttonScript.eventOnClick += SelectErase;
			gameObject.AddChild(buttonObj);
		}

		//Create available
		foreach(var tile in level.availableTiles)
		{
			var buttonObj = GameObject.Instantiate(buttonPrefab);
			var buttonScript = buttonObj.GetComponent<TileButton>();
			buttonScript.SetTile(tile);
			buttonScript.eventOnClick += SelectTile;
			gameObject.AddChild(buttonObj);
		}

		UpdateTileAmount();
	}
	public void OnDisable()
	{
		SelectTile(null);
	}
	public void Update()
	{
		if(cursorObj != null)
		{
			var gridPos = AppMain.MouseToGrid(Input.mousePosition);
			gridPos.x = Mathf.Floor(gridPos.x) + 0.5f;
			gridPos.y = Mathf.Floor(gridPos.y) + 0.5f;
			cursorObj.transform.position = gridPos;
		}

		if(Input.GetMouseButtonDown(0))
		{
			if (!AppMain.CheckIfMouseOverUI(Input.mousePosition))
			{
				//Find the tile position
				var gridPos = AppMain.MouseToGrid(Input.mousePosition);
				gridPos.x = Mathf.Floor(gridPos.x);
				gridPos.y = Mathf.Floor(gridPos.y);

				var tilePos = new Vector2Int((int)gridPos.x, (int)gridPos.y);

				var tilemap = AppMain.inst.level.tilemap;
				if(mouseMode == MouseMode.PAINT)
				{
					//Place
					if(currentTile != null)
						AppMain.inst.level.PlaceTile(currentTile, tilePos, cursorObj.transform.rotation);
				}
				else
				{
					//Remove
					AppMain.inst.level.RemoveTile(tilePos);
				}
				UpdateTileAmount();
			}
		}

		//Rotate
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
		{
			RotateTile();
		}
	}

	TileButton currentButton;
	GameTile currentTile;
	GameObject cursorObj;
	enum MouseMode
	{
		PAINT,
		ERASE
	}
	MouseMode mouseMode = MouseMode.PAINT;
	public void SelectTile(TileButton button)
	{
		//Cleanup prev obj
		if(cursorObj != null)
			GameObject.Destroy(cursorObj);
		if (currentButton != null)
			currentButton.SetSelected(false);

		//Set tile
		currentButton = button;
		currentTile = currentButton != null ? button.GetTile() : null;

		//Load new obj
		if(currentTile != null)
		{
			cursorObj = GameObject.Instantiate(currentTile.gameObject);
			mouseMode = MouseMode.PAINT;
		}
		
	}
	public void SelectErase(TileButton button)
	{
		SelectTile(button);
		mouseMode = MouseMode.ERASE;
	}
	public void RotateTile()
	{
		if (!cursorObj)
			return;

		cursorObj.transform.rotation = cursorObj.transform.rotation * Quaternion.AngleAxis(90.0f, Vector3.back);
	}
	public void UpdateTileAmount()
	{
		tileCountText.text = AppMain.inst.level.GetTilesLeft() + " Pieces Left";
	}
}
