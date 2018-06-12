using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
	public Stepp stepSequencer;

	public Sprite selectedSprite;
	public Sprite deselectedSprite;

	private bool selected = false;
	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			selected = value;
			
			SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
			spriteRenderer.sprite = (Selected) ? selectedSprite : deselectedSprite;
		}
	}

	public delegate void ButtonHandler(bool selected);
	public ButtonHandler MouseUpAsButton;

	private void OnMouseUpAsButton()
	{
		Selected = !Selected;
		if (MouseUpAsButton != null) {
			MouseUpAsButton(Selected);
		}
	}
}
