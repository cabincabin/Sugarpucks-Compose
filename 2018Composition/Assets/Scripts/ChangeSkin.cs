using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    //0 is default skin
    public List<Sprite> Skins;
    private Vector3 defaultSize;

    private void Start()
    {
        defaultSize = transform.localScale;
    }

    //0 is default
    public virtual void changeSkin(int SkinNum)
    {
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        if (SkinNum > Skins.Count || SkinNum < 0)
        {
            spriteRenderer.sprite = Skins[0];
            transform.localScale = new Vector3(defaultSize.x, defaultSize.y-.15f*SkinNum, defaultSize.z);
        }
        else
        {
            spriteRenderer.sprite = Skins[SkinNum];
            transform.localScale = defaultSize;
        }
    }
}