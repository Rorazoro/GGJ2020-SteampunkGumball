using UnityEngine;
using UnityEditor;

public struct SubType<BASE_TYPE>
{
	System.Type type;
	public SubType(System.Type type=null)
	{
		this.type = type;
	}
	public void Set(System.Type type)
	{
		this.type = type;
	}
	public void Set<IN_TYPE>() where IN_TYPE : BASE_TYPE
	{
		type = typeof(IN_TYPE);
	}
	public BASE_TYPE CreateInstance()
	{
		BASE_TYPE instance = (BASE_TYPE)System.Activator.CreateInstance(type);
		return instance;
	}
}