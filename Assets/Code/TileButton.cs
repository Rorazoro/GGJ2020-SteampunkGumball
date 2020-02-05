using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileButton : MonoBehaviour, IPointerClickHandler
{
	public EventSender<TileButton> eventOnClick = new EventSender<TileButton>();
	public UnityEngine.UI.Image image;
	public TMPro.TextMeshProUGUI amountText;
	public GameObject select;
	GameTile tile;

	public void SetTile(GameTile newTile)
	{
		this.tile = newTile;

		//Create graphic
		image.sprite = tile.sprite;

		//Update amount
		//amountText.text = tile.amount.ToString();
		//if (tile.amount <= -1)
		//	amountText.enabled = false;
	}
	public GameTile GetTile()
	{
		return tile;
	}
	public void OnPointerClick(PointerEventData pointerData)
	{
		eventOnClick.Invoke(this);
	}
	public void SetSelected(bool state)
	{
		select.SetActive(state);
	}
	public void OnDisable()
	{
		SetSelected(false);
	}
}
