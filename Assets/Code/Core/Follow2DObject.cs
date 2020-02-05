using UnityEngine;
using System.Collections;

public class Follow2DObject : MonoBehaviour
{
	public GameObject target;
	public Vector2 screenSpaceOffset = Vector2.zero;
	public bool checkWindowBounds = true;

	Canvas rootCanvas;

	void Start()
	{
		rootCanvas = gameObject.FindInParents<Canvas>();
		updatePosition();
	}
	void Update()
	{
		updatePosition();
	}
	void updatePosition()
	{
		var screenPos = target.transform.position;
		var position = screenPos + (Vector3)(screenSpaceOffset * rootCanvas.scaleFactor);
		transform.position = position;

		if(checkWindowBounds)
		{
			var rectTransform = transform as RectTransform;
			var rect = Game.Utility.RectTransformToScreenSpace(rectTransform);
			var windowRect = rootCanvas.pixelRect;

			//Check for correction
			var correctedRect = Game.Utility.KeepRectInsideRect(rect, windowRect);
			var offset = correctedRect.position - rect.position;
			if(offset != Vector2.zero)
				transform.position = position + (Vector3)offset;
		}
	}
}
