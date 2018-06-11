using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//When moving the timeline, the grid used also moves
//the distance it moves changes dynamically with the length of the grid used.
public class TimelinePositioning : MonoBehaviour{

    private Vector3 TimelinePoint;
    private Vector3 TimelineOffset;
    private float lenOfTimelineSeg = 13.36f;
    public List<GameObject> TimingGrids;
    private List<float> TimingGridDefaultLocation;
    
    void Start () {
        TimingGridDefaultLocation = new List<float>();
        for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
        {
            TimingGridDefaultLocation.Add(lenOfTimelineSeg*GridIndex);
        }
    }
 
    void Update () {
    }
    
    //On the mouse down, get the original location of the mouse as it corrisponds to the in game transformation
    //so that the mouse position does not have to be continueously transformed to an in game position.
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
        if (cursorPosition.x > -1.7 && cursorPosition.x < 10.84)
        {
            transform.position = cursorPosition;
            
            //transform each of the gridspaces
            for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
            {
                //multiply the timeline offset by the total length of the timing grids, so that, at the begining of the timeline
                //the grid will be at the start and at end of the timeline the grid will be at the end.
                //percent moved is how far along the timeline the time has moved
                float percentMoved = (cursorPosition.x + 1.74f) / 12.54f;
                //lenOfTimeline is the total length of the timeline
                float TotalToMoveGridspace = TimingGridDefaultLocation[GridIndex] - percentMoved * ((TimingGrids.Count-1) * 13.36f); 
                TimingGrids[GridIndex].transform.position = new Vector3(TotalToMoveGridspace, 0, 5);
            }
        }
        //else snap to max or min    
        else if (cursorPosition.x <= -1.7f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(-1.7f, cursorPosition.y, cursorPosition.z);
            for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
            {
                TimingGrids[GridIndex].transform.position = new Vector3(TimingGridDefaultLocation[GridIndex], 0, 5);  
            }
           
        }
        else if (cursorPosition.x >= 10.84f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(10.84f, cursorPosition.y, cursorPosition.z);
            for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
            {
                TimingGrids[GridIndex].transform.position = new Vector3(TimingGridDefaultLocation[GridIndex] - 13.36f*(TimingGrids.Count-1), 0, 5);  
            }
        }
    }
}
