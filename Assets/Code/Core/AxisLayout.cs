using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class AxisLayout : MonoBehaviour, ILayoutGroup
{
	public float padding = 0;
	public bool reverseOrder = false;

	public enum Axis
	{
		Horizontal,
		Vertical
	}
	public Axis axis = Axis.Horizontal;
	public enum Alignment
	{
		Front,
		Center,
		Back,
		Auto
	}
	public Alignment alignment = Alignment.Center;

	public void Start()
	{
		//Ask for rebuild
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
	}
	public void SetLayoutHorizontal()
	{
		if(axis != Axis.Horizontal)
			return;

		//Calculate static length
		float totalPercent = 0;
		int count = transform.childCount;
		float totalLength = 0;
		totalLength += padding * (count - 1);
		for(int i = 0; i < count; i++)
		{
			var child = transform.GetChild(i);

			//Perform child layout
			var layout = child.GetComponent<ILayoutSelfController>();
			if(layout != null)
				layout.SetLayoutHorizontal();

			var rectLayout = child.GetComponent<RectLayout>();
			if(rectLayout != null)
			{
				//Calc min rect
				var minRect = rectLayout.getMinRect();
				totalLength += minRect.width;

				//Add percent
				if(rectLayout.widthType == RectLayout.AxisType.Percent)
					totalPercent += rectLayout.width;
			}
			else
			{
				//Static value
				var rectTrans = child.transform as RectTransform;
				var rect = rectTrans.rect;
				totalLength += rect.width;
			}
		}

		//Remaining length
		var ourRect = (transform as RectTransform).rect;

		//Update percentage based children
		float remaining = Mathf.Max(ourRect.width - totalLength, 0);

		//Resize percentage children
		for(int i=0; i<count; i++)
		{
			var child = transform.GetChild(i);
			var rectLayout = child.GetComponent<RectLayout>();
			if(rectLayout != null && rectLayout.widthType == RectLayout.AxisType.Percent)
			{
				float ratio = rectLayout.width / totalPercent;
				rectLayout.setActualWidth(remaining * ratio);
			}
		}

		//Parent
		float autoPadding = 0;

		//Offset
		float offset = 0;
		if(alignment == Alignment.Front)
			offset = ourRect.width * -0.5f;
		else if(alignment == Alignment.Center)
			offset = totalLength * -0.5f;
		else if(alignment == Alignment.Back)
			offset = (ourRect.width * 0.5f) - totalLength;
		else if(alignment == Alignment.Auto)
		{
			offset = ourRect.width * -0.5f;
			autoPadding = (ourRect.width - totalLength) / (float)(count - 1);
			if(autoPadding < 0)
				autoPadding = 0;
		}

		//Space evenly
		float value = 0;
		for(int i = 0; i < count; i++)
		{
			var rectTrans = transform.GetChild(i).transform as RectTransform;
			var rect = rectTrans.rect;

			//Calc position
			var pos = rectTrans.anchoredPosition;
			pos.x = value + (rect.width * 0.5f) + offset;
			if(reverseOrder)
				pos.x = -pos.x;

			//Set position
			rectTrans.anchoredPosition = pos;

			//Add padding
			value += rect.width;
			value += padding;
			value += autoPadding;
		}
	}
	public void SetLayoutVertical()
	{
		if(axis != Axis.Vertical)
			return;

		//Calculate total height
		int count = transform.childCount;
		float totalLength = 0;
		totalLength += padding * (count - 1);
		for(int i=0; i<count; i++)
		{
			var child = transform.GetChild(i);

			//Perform child layout
			var layout = child.GetComponent<ILayoutSelfController>();
			if(layout != null)
				layout.SetLayoutVertical();

			//Rect layout
			var rectLayout = child.GetComponent<RectLayout>();
			if(rectLayout != null)
			{
				//Calc min rect
				var minRect = rectLayout.getMinRect();
				totalLength += minRect.height;
			}
			else
			{
				//Static value
				var rectTrans = child.transform as RectTransform;
				var rect = rectTrans.rect;
				totalLength += rect.height;
			}
		}

		//Parent
		var parentRect = (transform as RectTransform).rect;
		float autoPadding = 0;

		//Offset
		float offset = 0;
		if(alignment == Alignment.Front)
			offset = parentRect.height * -0.5f;
		else if(alignment == Alignment.Center)
			offset = totalLength * -0.5f;
		else if(alignment == Alignment.Back)
			offset = (parentRect.height * 0.5f) - totalLength;
		else if(alignment == Alignment.Auto)
		{
			offset = parentRect.height * -0.5f;
			autoPadding = (parentRect.height - totalLength) / (float)(count - 1);
			//if(autoPadding < 0)
			//	autoPadding = 0;
		}

		//Space evenly
		float value = 0;
		for(int i = 0; i < count; i++)
		{
			var rectTrans = transform.GetChild(i).transform as RectTransform;
			var rect = rectTrans.rect;

			//Calc position
			var pos = rectTrans.anchoredPosition;
			pos.y = value + (rect.height * 0.5f) + offset;
			if(!reverseOrder)
				pos.y = -pos.y;

			//Set position
			rectTrans.anchoredPosition = pos;

			//Add padding
			value += rect.height;
			value += padding;
			value += autoPadding;
		}
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

	public void markDirty()
	{
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);

		var parent = transform.parent.gameObject.GetComponent<AxisLayout>();
		if(parent != null)
			parent.markDirty();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(AxisLayout))]
	public class AxisLayoutEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if(EditorGUI.EndChangeCheck())
			{
				var script = this.target as AxisLayout;
				UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(script.transform as RectTransform);
			}
		}
	}
#endif
}
