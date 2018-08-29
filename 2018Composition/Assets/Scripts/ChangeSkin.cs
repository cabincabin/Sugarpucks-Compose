using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    //0 is default skin
    //list of all possible skins for the char/obj
    public List<Sprite> Skins;
    private Vector3 defaultSize;

    //on start, get the size of the puck, to keep the size consistant
    private void Start()
    {
        defaultSize = transform.localScale;
    }

    //0 is default
    //Changeskin takes a number relating to which skin to use
    //and changes the objects skin to said skin.
    public virtual void changeSkin(int SkinNum)
    {
        //get the local renderer
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        //if the number is outside the bounds
        if (SkinNum > Skins.Count || SkinNum < 0)
        {
            //show the default skin
            spriteRenderer.sprite = Skins[0];
            transform.localScale = new Vector3(defaultSize.x, defaultSize.y-.15f*SkinNum, defaultSize.z);
        }
        else
        {
            //show number selected skin
            spriteRenderer.sprite = Skins[SkinNum];
            transform.localScale = defaultSize;
        }
    }
}