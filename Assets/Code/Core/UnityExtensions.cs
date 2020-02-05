using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UnityExtensions
{
	//GameObject - Children
	static public GameObject GetParent(this GameObject obj)
	{
		return obj.transform.parent != null ? obj.transform.parent.gameObject : null;
	}
	static public T FindInParents<T>(this GameObject obj) where T : Component
	{
		if (obj == null) return null;
		var comp = obj.GetComponent<T>();

		if (comp != null)
			return comp;

		Transform t = obj.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}
	static public GameObject FindChild(this GameObject obj, string name, bool recursive=true)
	{
		if (obj == null)
			return null;

		//Check our children
		Transform findResult = obj.transform.Find(name);
		if (findResult)
			return findResult.gameObject;

		//Recursive
		if(recursive)
		{
			foreach (Transform child in obj.transform)
			{
				var result = child.gameObject.FindChild(name, recursive);
				if (result != null)
					return result;
			}
		}

		//Failed
		return null;
	}
	static public GameObject FindChildPath(this GameObject obj, string path)
	{
		if (obj == null)
			return null;
		var childTransform = obj.transform.Find(path);
		return childTransform != null ? childTransform.gameObject : null;
	}
	static public GameObject FindParent(this GameObject obj, string name)
	{
		if (obj == null)
			return null;

		if(obj.transform.parent != null)
		{
			var parent = obj.transform.parent.gameObject;
			if (parent.name == name)
				return parent;

			return parent.FindParent(name);
		}

		return null;
	}
	static public void AddChild(this GameObject obj, GameObject child, bool updatePosition=false)
	{
		if (obj == null || child == null)
		{
			Debug.Assert(false);
			return;
		}
		child.transform.SetParent(obj.transform, updatePosition);
	}
	static public void GetComponentsInChildrenRecursive<TYPE>(this GameObject obj, List<TYPE> output)
	{
		//Check children
		foreach(Transform childTrans in obj.transform)
		{
			var childObj = childTrans.gameObject;
			var comp = childObj.GetComponent<TYPE>();
			if (comp != null)
				output.Add(comp);
		}

		//Recursive
		foreach (Transform childTrans in obj.transform)
		{
			var childObj = childTrans.gameObject;
			childObj.GetComponentsInChildrenRecursive(output);
		}
	}
	static public void MoveToFront(this GameObject obj)
	{
		obj.transform.SetAsLastSibling();	
	}
	static public void MoveToBack(this GameObject obj)
	{
		obj.transform.SetAsFirstSibling();
	}
	static public Vector3 LocalToParent(this GameObject obj, Vector3 position, GameObject parent)
	{
		position = obj.transform.localToWorldMatrix.MultiplyPoint(position);
		position = parent.transform.worldToLocalMatrix.MultiplyPoint(position);
		return position;
	}
	static public void DestroyChildren(this GameObject obj, bool immediate=false)
	{
		if(immediate)
		{
			int count = obj.GetChildCount();
			for(int i=0; i<count; i++)
				GameObject.DestroyImmediate(obj.GetChild(0));
		}
		else
		{
			foreach(Transform childTrans in obj.transform)
				GameObject.Destroy(childTrans.gameObject);
		}
	}
	static public int GetChildCount(this GameObject obj)
	{
		return obj.transform.childCount;
	}
	static public GameObject GetChild(this GameObject obj, int index)
	{
		return obj.transform.GetChild(index).gameObject;
	}

	//GameObject - Components
	static public T GetOrAdd<T>(this GameObject obj) where T : MonoBehaviour
	{
		var comp = obj.GetComponent<T>();
		if(comp == null)
			comp = obj.AddComponent<T>();
		return comp;
	}

	private static System.Random rng = new System.Random();
	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

    public static Vector2 XY(this Vector3 value)
    {
        return new Vector2(value.x, value.y);
    }

	public static TYPE Random<TYPE>(this List<TYPE> value, TYPE nullValue=default(TYPE))
	{
		if(value.Count == 0)
			return nullValue;
		else
			return value[UnityEngine.Random.Range(0, value.Count)];
	}
	public static IEnumerable<TYPE> RandomRange<TYPE>(this List<TYPE> list, int amount)
	{
		List<TYPE> result = new List<TYPE>(list);
		int difference = list.Count - amount;
		for(int i=0; i<difference; i++)
			result.RemoveAt(Game.Random.Range(0, result.Count-1));

		return result;
	}
	public static void Randomize<TYPE>(this List<TYPE> list)
	{
		int size = list.Count;
		for(int index=0; index<size; index++)
		{
			int randIndex = UnityEngine.Random.Range(0, size);
			var valueA = list[index];
			var valueB = list[randIndex];

			//Swap
			list[index] = valueB;
			list[randIndex] = valueA;
		}
	}

	//RectTrasform
	public static void SetSize(this RectTransform transform, float width, float height)
	{
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
	}

	//Color
	public static Color RGBtoHSV(this Color color)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		return new Color(h, s, v, color.a);
	}
	public static Color HSVtoRGB(this Color color)
	{
		var result = Color.HSVToRGB(color.r, color.g, color.b);
		result.a = color.a;
		return result;
	}
	public static Color AdjustHSLMultiply(this Color color, float hue, float saturation, float value)
	{
		var result = color.RGBtoHSV();
		result.r *= hue;
		result.g *= saturation;
		result.b *= value;
		return result.HSVtoRGB();
	}

	//List
	public static List<T> SubList<T>(this List<T> data, int index, int length)
	{
		List<T> result = new List<T>(length);
		for(int i=0; i<length; i++)
			result.Add(data[index + i]);
		return result;
	}
	public static List<T> SubList<T>(this List<T> data, int index)
	{
		int length = data.Count - index;
		List<T> result = new List<T>(length);
		for(int i = 0; i < length; i++)
			result.Add(data[index + i]);
		return result;
	}

	//Array
	public static bool Contains<T>(this T[] array, T obj) where T : class
	{
		foreach(var value in array)
		{
			if(value == obj)
				return true;
		}
		return false;
	}
	public static T[] SubArray<T>(this T[] data, int index, int length)
	{
		T[] result = new T[length];
		System.Array.Copy(data, index, result, 0, length);
		return result;
	}
	public static T[] SubArray<T>(this T[] data, int index)
	{
		int length = data.Length - index;
		T[] result = new T[length];
		System.Array.Copy(data, index, result, 0, length);
		return result;
	}

	//String
	public static string ToCamelCase(this string str)
	{
		if(!string.IsNullOrEmpty(str) && str.Length > 1)
		{
			return System.Char.ToUpper(str[0]) + str.Substring(1).ToLower();
		}
		return str;
	}
}

