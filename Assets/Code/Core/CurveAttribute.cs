using UnityEngine;
using UnityEditor;

public class CurveAttribute : PropertyAttribute
{
	public float PosX, PosY;
	public float RangeX, RangeY;
	public bool b;
	public int x;

	public CurveAttribute(float PosX, float PosY, float RangeX, float RangeY, bool b)
	{
		this.PosX = PosX;
		this.PosY = PosY;
		this.RangeX = RangeX;
		this.RangeY = RangeY;
		this.b = b;
	}
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CurveAttribute))]
public class CurveDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		CurveAttribute curve = attribute as CurveAttribute;
		if (property.propertyType == SerializedPropertyType.AnimationCurve)
		{
			if (curve.b) EditorGUI.CurveField(position, property, Color.cyan, new Rect(curve.PosX, curve.PosY, curve.RangeX, curve.RangeY));
		}
	}
}

#endif
