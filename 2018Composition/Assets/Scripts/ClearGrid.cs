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
    public TimelinePositioning timeline;

    private void OnMouseUpAsButton()
    {
        Clear();
    }

    public void Clear()
    {
        List<GameObject> GridsAsGameObj = timeline.TimingGrids;
        foreach (var GridGameObj in GridsAsGameObj)
        {
            TimingGrid Grid = GridGameObj.GetComponent<TimingGrid>();
            foreach (var PlayPuck in Grid.Sprites)
            {
                PlayPuck.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(PlayPuck);   
            }

            Grid.Sprites = new List<Collider2D>();
        }
    }
}