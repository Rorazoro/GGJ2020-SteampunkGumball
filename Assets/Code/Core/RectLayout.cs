using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class RectLayout : MonoBehaviour, ILayoutSelfController
{
	public struct Rect
	{
		public Vector2 min;
		public Vector2 max;

		public float width
		{
			get
			{
				return max.x - min.x;
			}
		}
		public float height
		{
			get
			{
				return max.y - min.y;
			}
		}
	}

	public float paddingL = 0;
	public float paddingR = 0;
	public float paddingT = 0;
	public float paddingB = 0;

	float marginL = 0;
	float marginR = 0;
	float marginT = 0;
	float marginB = 0;

	//Width/Height -----------------------------------------------

	public float width = 1;
	public float height = 1;

	public enum AxisType
	{
		Static,
		Percent,
		Child,
		None
	}
	public AxisType widthType = AxisType.Percent;
	public AxisType heightType = AxisType.Percent;

	public void setWidth(float value, AxisType axisType = AxisType.Static)
	{
		//Store
		width = value;
		widthType = axisType;

		//Mark
		markDirty();
	}
	public void setHeight(float value, AxisType axisType = AxisType.Static)
	{
		//Store
		height = value;
		heightType = axisType;

		//Mark
		markDirty();
	}

	//Convenience
	public Rect getMinRect()
	{
		//Width
		float minWidth = 0;
		if(widthType == AxisType.Static)
			minWidth = width;
		else if(widthType == AxisType.Percent)
			minWidth = 0;
		else if(widthType == AxisType.Child)
		{
			var rectTransform = transform as RectTransform;
			minWidth = rectTransform.rect.width;
		}

		//Height
		float minHeight = 0;
		if(heightType == AxisType.Static)
			minHeight = height;
		else if(heightType == AxisType.Percent)
			minHeight = 0;
		else if(heightType == AxisType.Child)
		{
			var rectTransform = transform as RectTransform;
			minWidth = rectTransform.rect.height;
		}

		Rect rect = new Rect();
		rect.max.x = minWidth;
		rect.max.y = minHeight;
		return rect;
	}
	public void setActualWidth(float value)
	{
		(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
	}
	public void setActualHeight(float value)
	{
		(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
	}

	//Metadata
	public void setPivot(Vector2 pivot)
	{
		var rect = transform as RectTransform;
		rect.pivot = pivot;
	}
	public void setPadding(float value)
	{
		//Store
		paddingL = value;
		paddingR = value;
		paddingT = value;
		paddingB = value;

		//Mark
		markDirty();
	}
	public void setPadding(float paddingL, float paddingR, float paddingT, float paddingB)
	{
		//Store
		this.paddingL = paddingL;
		this.paddingR = paddingR;
		this.paddingT = paddingT;
		this.paddingB = paddingB;

		//Mark
		markDirty();
	}

	public void markDirty()
	{
		UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);

		var parent = transform.parent.gameObject.GetComponent<AxisLayout>();
		if(parent != null)
			parent.markDirty();
	}

	//ILayoutSelfController
	public void SetLayoutHorizontal()
	{
		var rectTransform = transform as RectTransform;
		if(widthType == AxisType.Static)
		{
			//Set static size
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		}
		else if(widthType == AxisType.Percent)
		{
			var parent = transform.parent.gameObject.GetComponent<AxisLayout>();
			if(parent == null || parent.axis != AxisLayout.Axis.Horizontal)
			{
				var parentRect = transform.parent.transform as RectTransform;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentRect.rect.width);
			}
		}
		else if(widthType == AxisType.Child)
		{
			//Sort children
			{
				var axisLayout = gameObject.GetComponent<AxisLayout>();
				if(axisLayout != null)
					axisLayout.SetLayoutHorizontal();
			}

			//Find maximum child
			float minValue = float.MaxValue;
			float maxValue = float.MinValue;

			bool hasChildren = false;
			foreach(RectTransform childTrans in rectTransform)
			{
				var layout = childTrans.gameObject.GetComponent<ILayoutSelfController>();
				if(layout != null)
					layout.SetLayoutVertical();

				var rect = childTrans.rect;
				minValue = Mathf.Min(rect.xMin - childTrans.anchoredPosition.x, minValue);
				maxValue = Mathf.Max(rect.xMax - childTrans.anchoredPosition.x, maxValue);
				hasChildren = true;
			}
			if(!hasChildren)
			{
				minValue = 0;
				maxValue = 0;
			}

			float totalValue = maxValue - minValue;
			totalValue += paddingL + paddingR;

			//Set
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalValue);
		}
	}
	public void SetLayoutVertical()
	{
		var rectTransform = transform as RectTransform;
		if(heightType == AxisType.Static)
		{
			//Set static size
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}
		else if(heightType == AxisType.Percent)
		{
			var parent = transform.parent.gameObject.GetComponent<AxisLayout>();
			if(parent == null || parent.axis != AxisLayout.Axis.Vertical)
			{
				var parentRect = transform.parent.transform as RectTransform;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRect.rect.height);
			}
		}
		else if(heightType == AxisType.Child)
		{
			//Sort children
			{
				var axisLayout = gameObject.GetComponent<AxisLayout>();
				if(axisLayout != null)
					axisLayout.SetLayoutVertical();
			}

			//Find maximum child
			float minValue = float.MaxValue;

			float maxValue = float.MinValue;

			bool hasChildren = false;
			foreach(RectTransform childTrans in rectTransform)
			{
				var layout = childTrans.gameObject.GetComponent<ILayoutSelfController>();
				if(layout != null)
					layout.SetLayoutVertical();

				var rect = childTrans.rect;
				minValue = Mathf.Min(rect.yMin - childTrans.anchoredPosition.y, minValue);
				maxValue = Mathf.Max(rect.yMax - childTrans.anchoredPosition.y, maxValue);
				hasChildren = true;
			}
			if(!hasChildren)
			{
				minValue = 0;
				maxValue = 0;
			}

			float totalValue = maxValue - minValue;
			totalValue += paddingT + paddingB;

			//Set
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalValue);

			/*float totalLength = 0;

			//Padding
			totalLength += paddingT + paddingB;

			//Children
			foreach(RectTransform childTrans in rectTransform)
			{
				totalLength += childTrans.rect.height;
			}

			//Set value
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalLength);*/
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(RectLayout))]
	public class RectLayoutEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if(EditorGUI.EndChangeCheck())
			{
				var script = this.target as RectLayout;
				script.markDirty();
			}
		}
	}
#endif
}
