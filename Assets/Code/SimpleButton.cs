using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleButton : MonoBehaviour, IPointerClickHandler
{
	public UnityEvent eventOnClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventOnClick != null)
			eventOnClick.Invoke();
	}
}
