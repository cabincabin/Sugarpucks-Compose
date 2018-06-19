using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//When moving the timeline, the grid used also moves
//the distance it moves changes dynamically with the length of the grid used.
public class TimelinePositioning : MonoBehaviour{
    
    public int measures;
    public GameObject Grid;
    private Vector3 TimelinePoint;
    private Vector3 TimelineOffset;
    private float lenOfTimelineSeg = 1.4f;
    private int segmentsPerScreen = 11;
    
    //do not add anything to this public obj
    public List<GameObject> TimingGrids;
    private List<float> TimingGridDefaultLocation;
    public DeleteOnContact PianoContact;
    
    void Start ()
    {
        //fill at least 1 screen worth with mesures.
        if (measures < segmentsPerScreen)
            measures = segmentsPerScreen;
        
       
        TimingGrids = new List<GameObject>();
        TimingGridDefaultLocation = new List<float>();
        
        //add initial targets
        TimingGrids.Add(Grid);
        TimingGridDefaultLocation.Add(0);
        
        //create the number of mesures and put them in place
        for (int GridIndex = 1; GridIndex < measures; GridIndex++)
        {
            //place each grid after the previous grid with 0 rotation
            TimingGrids.Add(Instantiate(Grid, new Vector3(lenOfTimelineSeg*GridIndex, 0f , 10f), Quaternion.identity));
            //get the default location through simple multiplication
            TimingGridDefaultLocation.Add(lenOfTimelineSeg*GridIndex);
        }
    }
 
    //On the mouse down, get the original location of the mouse as it corrisponds to the in game transformation
    //so that the mouse position does not have to be continueously transformed to an in game position.
    void OnMouseDown()
    {
        //get the timeline's position
        TimelinePoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //change only the x TimelineOffset.
        TimelineOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, TimelinePoint.y, TimelinePoint.z));
        PianoContact.destroyAll = true;
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
                float TotalToMoveGridspace = TimingGridDefaultLocation[GridIndex] - percentMoved * ((TimingGrids.Count-segmentsPerScreen) * lenOfTimelineSeg); 
                TimingGrids[GridIndex].transform.position = new Vector3(TotalToMoveGridspace, 0, 10);
            }
        }
        //else snap to max or min    
        else if (cursorPosition.x <= -1.7f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(-1.7f, cursorPosition.y, cursorPosition.z);
            for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
            {
                TimingGrids[GridIndex].transform.position = new Vector3(TimingGridDefaultLocation[GridIndex], 0, 10);  
            }
           
        }
        else if (cursorPosition.x >= 10.84f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(10.84f, cursorPosition.y, cursorPosition.z);
            for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
            {
                TimingGrids[GridIndex].transform.position = new Vector3(TimingGridDefaultLocation[GridIndex] - lenOfTimelineSeg*(TimingGrids.Count-segmentsPerScreen), 0, 10);  
            }
        }
    }

    
}
