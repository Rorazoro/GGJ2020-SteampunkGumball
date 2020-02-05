using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Game
{
	public static class Editor
	{
		public static void DrawSceneRect(Rect rect)
		{
			Handles.DrawLine(new Vector2(rect.min.x, rect.min.y), new Vector2(rect.min.x, rect.max.y));
			Handles.DrawLine(new Vector2(rect.min.x, rect.max.y), new Vector2(rect.max.x, rect.max.y));
			Handles.DrawLine(new Vector2(rect.max.x, rect.max.y), new Vector2(rect.max.x, rect.min.y));
			Handles.DrawLine(new Vector2(rect.max.x, rect.min.y), new Vector2(rect.min.x, rect.min.y));
		}
	}

	public static class EditorUI
	{
		public static string TextArea(string label, string text, Vector2 scrollPos, int height=200, bool wordWrap=true)
		{
			//Text Area
			EditorGUILayout.LabelField(label);
			EditorStyles.textField.wordWrap = wordWrap;
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(height));
			text = EditorGUILayout.TextArea(text, GUILayout.MaxHeight(height-6));
			EditorGUILayout.EndScrollView();

			return text;
		}

	}

	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			EditorGUI.BeginChangeCheck();
			_property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
			if(EditorGUI.EndChangeCheck())
			{
				Debug.Log(_property.intValue);
			}
		}
	}

	/*[CustomPropertyDrawer(typeof(InspectorDropList))]
	public class InspectorDropListDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			var attrib = attribute as InspectorDropList;

			//Add items
			{
				var item = EditorGUILayout.ObjectField(_property.displayName, null, attrib.objType, attrib.allowSceneObjects);
				if(item != null)
				{
					var index = _property.arraySize;
					_property.InsertArrayElementAtIndex(index);
					_property.GetArrayElementAtIndex(index).objectReferenceValue = item;
				}
			}

			//Display items
			int size = _property.arraySize;
			for(int i = 0; i < size; i++)
			{
				var item = _property.GetArrayElementAtIndex(i);
				item.objectReferenceValue = EditorGUILayout.ObjectField(item.objectReferenceValue, attrib.objType, attrib.allowSceneObjects);
			}
		}
	}*/

}
