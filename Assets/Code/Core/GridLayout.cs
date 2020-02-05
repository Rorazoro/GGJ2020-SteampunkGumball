using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class GridLayout : MonoBehaviour, UnityEngine.UI.ILayoutGroup
{
	public int cellWidth = 100;
	public int cellHeight = 100;
	public bool resizeHeight = false;

	public enum AlignH
	{
		LEFT,
		CENTER,
		RIGHT
	}
	public AlignH horizontalAlign = AlignH.CENTER;
	public enum AlignV
	{
		TOP,
		CENTER,
		BOTTOM
	}
	public AlignV verticalAlign = AlignV.CENTER;

	public void Start()
	{
		//Ask for rebuild
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
	}
	public void SetLayoutHorizontal()
	{
		//Determine metrics
		int childCount = gameObject.GetChildCount();
		var rect = (transform as RectTransform).rect;

		int columns = (int)Mathf.Floor((float)rect.width / (float)cellWidth);
		int rows = (int)(Mathf.Ceil((float)childCount / (float)columns));

		columns = Mathf.Max(1, columns);
		rows = Mathf.Max(1, rows);

		int totalCellWidth = columns * cellWidth;
		int totalCellHeight = rows * cellHeight;

		//Resize self
		if(resizeHeight)
			(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalCellHeight);

		var alignOffset = Vector2.zero;

		//Pivot offset
		Vector2 pivotOffset = Vector2.zero;
		pivotOffset.x = (transform as RectTransform).pivot.x * -rect.width;
		pivotOffset.y = (1.0f - (transform as RectTransform).pivot.y) * rect.height;

		//Horizontal Alignment
		if(horizontalAlign == AlignH.LEFT)
			alignOffset.x = 0;
		else if(horizontalAlign == AlignH.CENTER)
			alignOffset.x = (rect.width * 0.5f) - (totalCellWidth * 0.5f);
		else if(horizontalAlign == AlignH.RIGHT)
			alignOffset.x = (rect.width - totalCellWidth);

		//Vertical Alignment
		if(verticalAlign == AlignV.TOP)
			alignOffset.y = 0;
		else if(verticalAlign == AlignV.CENTER)
			alignOffset.y = (rect.height * 0.5f) - (totalCellHeight * 0.5f);
		else if(verticalAlign == AlignV.BOTTOM)
			alignOffset.y = (rect.height - totalCellHeight);

		//Horizontal Alignment
		/*if(horizontalAlign == AlignH.LEFT)
			alignOffset.x = rect.width * -0.5f;
		else if(horizontalAlign == AlignH.CENTER)
			alignOffset.x = totalCellWidth * -0.5f;
		else if(horizontalAlign == AlignH.RIGHT)
			alignOffset.x = -(totalCellWidth - (rect.width * 0.5f));

		//Vertical Alignment
		if(verticalAlign == AlignV.TOP)
			alignOffset.y = rect.height * 0.5f;
		else if(verticalAlign == AlignV.CENTER)
			alignOffset.y = totalCellHeight * 0.5f;
		else if(verticalAlign == AlignV.BOTTOM)
			alignOffset.y = (totalCellHeight - (rect.height * 0.5f));*/

		var cellOffset = new Vector2(cellWidth * 0.5f, -cellHeight * 0.5f);

		for(int childIter=0; childIter<childCount; childIter++)
		{
			var child = gameObject.GetChild(childIter);

			var position = pivotOffset + alignOffset + cellOffset;
			position.x += (childIter % columns) * cellWidth;
			position.y += (childIter / columns) * -cellHeight;

			child.transform.localPosition = position;
		}
	}
	public void SetLayoutVertical()
	{
	}
	public void OnTransformChildrenChanged()
	{
		//Ask for rebuild
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
	}
	public void OnRectTransformDimensionsChange()
	{
		//Ask for rebuild
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(GridLayout))]
	public class AxisLayoutEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if(EditorGUI.EndChangeCheck())
			{
				var script = this.target as GridLayout;
				UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(script.transform as RectTransform);
			}
		}
	}
#endif
}
