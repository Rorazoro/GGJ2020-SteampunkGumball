using UnityEngine;

namespace Game
{
	public class EnumFlagsAttribute : PropertyAttribute
	{
		public EnumFlagsAttribute() { }
	}
	/*public class InspectorDropList : PropertyAttribute
	{
		public System.Type objType;
		public bool allowSceneObjects;
		public InspectorDropList(System.Type objType, bool allowSceneObjects=false)
		{
			this.objType = objType;
			this.allowSceneObjects = allowSceneObjects;
		}
	}*/
}