using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
	public GameObject levelButtonPrefab;

    void Start()
    {
		foreach(var level in AppMain.inst.levels)
		{
			var obj = GameObject.Instantiate(levelButtonPrefab);
			var text = obj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
			text.text = level.name;
			var button = obj.GetComponent<SimpleButton>();
			button.eventOnClick.AddListener(()=>
			{
				AppMain.inst.LoadLevel(level);
			});

			gameObject.AddChild(obj);
		}
    }
}
