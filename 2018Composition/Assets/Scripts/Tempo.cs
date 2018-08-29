using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tempo : MonoBehaviour
{
    public Stepp PlaySequenceTempo; 
    private Vector3 TimelinePoint;
    private Vector3 TimelineOffset;
    public int MaxTempo;
    public int MinTempo;

    private void Start()
    {
        //start at the slowest tempo
        PlaySequenceTempo.tempo = MinTempo;
    }

    void OnMouseDown()
    {
        //get the timeline's position
        TimelinePoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //change only the x TimelineOffset.
        TimelineOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, TimelinePoint.y, TimelinePoint.z));
        
    }
    
    //update the timeline position and move the TimingGrids by the relitive distance needed for transform
    void OnMouseDrag(){
        
        //update timeline position
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, TimelinePoint.y, TimelinePoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + TimelineOffset;
        
      
        //bound grid and timeline
        //if in bounds move freely
        if (cursorPosition.x >= 0 && cursorPosition.x <= .8)
        {
            transform.position = cursorPosition;
        }
        
        //else snap to max or min    
        else if (cursorPosition.x < 0f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(0f, cursorPosition.y, cursorPosition.z);
           
        }
        else if (cursorPosition.x > .8f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(.8f, cursorPosition.y, cursorPosition.z);
    
        }
    
        PlaySequenceTempo.tempo = (int) (MinTempo + ((MaxTempo-MinTempo)*transform.position.x / .8f));
    }

}
