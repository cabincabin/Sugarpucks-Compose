using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClearGrid : MonoBehaviour
{
    //could be made singleton
    public TimelinePositioning timeline;

    //when clear is clicked, clear all notes on the screen
    private void OnMouseUpAsButton()
    {
        Clear();
    }

    //allow clear to be called externally
    public void Clear()
    {
        //for each timeline grid
        List<GameObject> GridsAsGameObj = timeline.TimingGrids;
        foreach (var GridGameObj in GridsAsGameObj)
        {
            TimingGrid Grid = GridGameObj.GetComponent<TimingGrid>();
            foreach (var PlayPuck in Grid.Sprites)
            {
                //destroy the objects  in the grid
                PlayPuck.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(PlayPuck);   
            }

            //create a new list without any objects, so that new objects can be added. 
            Grid.Sprites = new List<Collider2D>();
        }
    }
}