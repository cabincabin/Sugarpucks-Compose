using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Key : MonoBehaviour
{
    //should be 12, one for each piano puck
    //should be in order from a to g# with a being the first number
    //this could be confirmed using the numbers of each sprite if this is too much effort
    public List<GameObject> PianoPucks;
    //0 should be word key
    //1 - 12 should be a through g#
    public List<Sprite> KeyIcon;
    public DeleteOnContact PianoContact;

    //do not touch
    public bool hasKey;
    //piano pucks default location
    private float defaultx = -8;
    private bool CanChooseKey;
    //do not touch
    public List<int> NumInKey;
    
    


    //when selected allow for key change
    private void OnMouseUpAsButton()
    {
        CanChooseKey = true;
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        //render word key
        spriteRenderer.sprite = KeyIcon[0];
    }

    //allows piano puck to choose key
    public void chooseKey(int KeyNum)
    {
        //if a key can be and is chosen
        if (CanChooseKey)
        {
            //show that a key has been choosen
            hasKey = true;
            NumInKey = new List<int>
            {
                KeyNum, (KeyNum+2)%12, (KeyNum+4)%12, (KeyNum+5)%12, (KeyNum+7)%12, (KeyNum+9)%12, (KeyNum+11)%12
            };
            //reset position of all pucks
            foreach (var Puck in PianoPucks)
            {
                Puck.transform.position = new Vector3(defaultx,  Puck.transform.position.y,  Puck.transform.position.z);
                Puck.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            }

            foreach (var Num in NumInKey)
            {
                PianoPucks[Num].transform.position = new Vector3(defaultx + 1f,  PianoPucks[Num].transform.position.y,  PianoPucks[Num].transform.position.z);
                PianoPucks[Num].GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            //shift first puck over
           
            CanChooseKey = false;
            
            SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
            //render keyname
            spriteRenderer.sprite = KeyIcon[KeyNum+1];
            PianoContact.destroyAll = true;
        }
    }
}