using UnityEngine;
using UnityEditor;

public class LoadLevel : Game.Action
{
	GameObject levelPrefab;
	public LoadLevel(GameObject levelPrefab)
	{
		this.levelPrefab = levelPrefab;
	}
	public override void Update(float timeElapsed)
	{
		AppMain.inst.LoadLevelRaw(levelPrefab);

		isComplete = true;
	}
}

public class BeginLevel : Game.Action
{
	public override void Update(float timeElapsed)
	{
		AppMain.inst.Cleanup();
		AppMain.inst.level.Init();
		AppMain.inst.editMode.enabled = true;

		isComplete = true;
	}
}