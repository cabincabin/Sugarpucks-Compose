using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

//When moving the timeline, the grid used also moves
//the distance it moves changes dynamically with the length of the grid used.
public class TimelinePositioning : MonoBehaviour{
    
    public int measures;
    public GameObject Grid;
    private Vector3 TimelinePoint;
    private Vector3 TimelineOffset;
    private float lenOfTimelineSeg = 1.4f;
    private int segmentsPerScreen = 11;
    public int TimeSig;
    public GameObject P1;
    public GameObject P4;
    public GameObject PX;
    
    //do not add anything to this public obj
    public List<GameObject> TimingGrids;
    private List<float> TimingGridDefaultLocation;
    private float AddMesureDefaultPosition;
    
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
            GameObject NextGrid = Instantiate(Grid, new Vector3(lenOfTimelineSeg * GridIndex, 0f, 10f),
                Quaternion.identity);
            if(GridIndex%TimeSig!=0)
                NextGrid.transform.localScale = new Vector3(NextGrid.transform.localScale.x,NextGrid.transform.localScale.y*.8f,NextGrid.transform.localScale.z);
            TimingGrids.Add(NextGrid);
            //get the default location through simple multiplication
            TimingGridDefaultLocation.Add(lenOfTimelineSeg*GridIndex);
        }

        AddMesureDefaultPosition = measures * lenOfTimelineSeg;
        Vector3 PosOfAdd = new Vector3(AddMesureDefaultPosition, 0f, 0f);
        P1.transform.position = PosOfAdd;
        P4.transform.position = PosOfAdd;
        PX.transform.position = PosOfAdd;

    }

    public void AppendMesure()
    {
        measures++;
        GameObject NextGrid = Instantiate(Grid, new Vector3(lenOfTimelineSeg * (measures-1), 0f, 10f),
            Quaternion.identity);
        if((measures-1)%TimeSig!=0)
            NextGrid.transform.localScale = new Vector3(NextGrid.transform.localScale.x,NextGrid.transform.localScale.y*.8f,NextGrid.transform.localScale.z);
        TimingGrids.Add(NextGrid);
        //get the default location through simple multiplication
        TimingGridDefaultLocation.Add(lenOfTimelineSeg*(measures-1));
        AddMesureDefaultPosition = measures * lenOfTimelineSeg;
        updateTimelinePos();
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
        }
        //else snap to max or min    
        else if (cursorPosition.x <= -1.7f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(-1.7f, cursorPosition.y, cursorPosition.z);
        }
        else if (cursorPosition.x >= 10.84f)
        {
            //transform each of the gridspaces
            transform.position = new Vector3(10.84f, cursorPosition.y, cursorPosition.z);
        }

        updateTimelinePos();
    }

    private void updateTimelinePos()
    {
        float percentMoved = (transform.position.x + 1.74f) / 12.54f;
        //transform each of the gridspaces
        for (int GridIndex = 0; GridIndex < TimingGrids.Count; GridIndex++)
        {
            //multiply the timeline offset by the total length of the timing grids, so that, at the begining of the timeline
            //the grid will be at the start and at end of the timeline the grid will be at the end.
            //percent moved is how far along the timeline the time has moved
            
            //lenOfTimeline is the total length of the timeline
            float TotalToMoveGridspace = TimingGridDefaultLocation[GridIndex] - percentMoved * ((TimingGrids.Count+1-(segmentsPerScreen)) * lenOfTimelineSeg); 
            TimingGrids[GridIndex].transform.position = new Vector3(TotalToMoveGridspace, 0, 10);
        }
        Vector3 PosOfAdd = new Vector3(AddMesureDefaultPosition- percentMoved * ((TimingGrids.Count+1-(segmentsPerScreen)) * lenOfTimelineSeg), 0f, 0f);
        P1.transform.position = PosOfAdd;
        P4.transform.position = PosOfAdd;
        PX.transform.position = PosOfAdd;


    }


}
