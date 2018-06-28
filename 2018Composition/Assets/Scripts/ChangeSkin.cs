using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    //0 is default skin
    public List<Sprite> Skins;

    //0 is default
    public virtual void changeSkin(int SkinNum)
    {
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        if (SkinNum > Skins.Count || SkinNum < 0)
        {

            spriteRenderer.sprite = Skins[0];
        }
        else
        {
            spriteRenderer.sprite = Skins[SkinNum];
        }
    }
}