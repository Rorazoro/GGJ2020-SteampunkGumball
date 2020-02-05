// Initial Concept by http://www.reddit.com/user/zaikman
// Revised by http://www.reddit.com/user/quarkism

using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class EditorButton : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var mono = target as MonoBehaviour;

		var methods = mono.GetType()
			.GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
						BindingFlags.NonPublic)
			.Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));

		foreach (var memberInfo in methods)
		{
			if (GUILayout.Button(memberInfo.Name))
			{
				var method = memberInfo as MethodInfo;
				method.Invoke(mono, null);
			}
		}
	}
}
#endif