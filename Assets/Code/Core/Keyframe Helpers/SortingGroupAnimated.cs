using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UnityEngine.Rendering.SortingGroup))]
public class SortingGroupAnimated : MonoBehaviour
{
	UnityEngine.Rendering.SortingGroup group;
	public void OnEnable()
	{
		group = GetComponent<UnityEngine.Rendering.SortingGroup>();
	}
	public void SetSortingOrder(int sortingOrder)
	{
		group.sortingOrder = sortingOrder;
	}
}
