using UnityEngine;
using System.Collections.Generic;
using System;

public class GameLevel : MonoBehaviour
{
	public UnityEngine.Tilemaps.Tilemap tilemap;
	[NonSerialized] public GameObject ourPrefab;

	StartTile startTile;

	public Vector3 GetBallStartPos()
	{
		return startTile.transform.position;
	}

	public void Init()
	{
		tilemap = GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
		startTile = GetComponentInChildren<StartTile>();

		int count = tilemap.gameObject.GetChildCount();
	}

	public GameObject EjectBall()
	{
		return startTile.EjectBall();
	}

	List<Vector2Int> placedTiles = new List<Vector2Int>();
	public void PlaceTile(GameTile tile, Vector2Int pos, Quaternion rotation)
	{
		//Check if it already exists
		if (placedTiles.Contains(pos))
			return;
		if (tilemap.HasTile((Vector3Int)pos))
			return;
		if (GetTilesLeft() <= 0)
			return;

		//Add
		tilemap.SetTile((Vector3Int)pos, tile);
		placedTiles.Add(pos);

		//Rotate
		var instance = tilemap.GetInstantiatedObject((Vector3Int)pos);
		instance.transform.rotation = rotation;
	}
	public void RemoveTile(Vector2Int pos)
	{
		//Check if it's placed
		if (!placedTiles.Contains(pos))
			return;

		//Remove
		placedTiles.Remove(pos);
		tilemap.SetTile((Vector3Int)pos, null);
	}
	public int GetTilesLeft()
	{
		return maxPieces - placedTiles.Count;
	}

	//Available Tiles
	public List<GameTile> availableTiles = new List<GameTile>();
	public int maxPieces = 10;
}
