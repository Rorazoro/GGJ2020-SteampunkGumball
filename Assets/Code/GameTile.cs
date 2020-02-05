using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
[CreateAssetMenu(menuName = "Game/Game Tile")]
public class GameTile : UnityEngine.Tilemaps.Tile
{
	void Start()
	{
		
	}
	public void StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		if(go != null)
		{
			go.transform.rotation = tilemap.GetTransformMatrix(position).rotation;
		}
	}
}
