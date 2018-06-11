using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//When moving the timeline, the grid used also moves
//the distance it moves changes dynamically with the length of the grid used.
public class TimelinePositioningCopy : MonoBehaviour{

    private Vector3 TimelinePoint;
    private Vector3 TimelineOffset;
    public GameObject TimingGrid;
    private Vector3 TimingGridscreenPoint;
    private Vector3 TimingGridOffset;
    
    void Start () {
        Debug.Log("AHH");
    }
 
    void Update () {
    }
    
    //On the mouse down, get the original location of the mouse as it corrisponds to the in game transformation
    //so that the mouse position does not have to be continueously transformed to an in game position.
    void OnMouseDown()
    {
        Debug.Log("Here");
        //get the timeline's position
        TimelinePoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //change only the x TimelineOffset.
        TimelineOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, TimelinePoint.y, TimelinePoint.z));

        TimingGridscreenPoint = Camera.main.WorldToScreenPoint(TimingGrid.transform.position);
        //change only the x offset.
        TimingGridOffset = TimingGrid.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, TimingGridscreenPoint.y, TimingGridscreenPoint.z));
        
    }
    
    //update the timeline position and move the TimingGrids by the relitive distance needed for transform
    void OnMouseDrag(){
        
        //update timeline positin
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, TimelinePoint.y, TimelinePoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + TimelineOffset;
        
        //update grid position
        Vector3 TimingGridcursorPoint = new Vector3(Input.mousePosition.x, TimingGridscreenPoint.y, TimingGridscreenPoint.z);
        Vector3 TimingGridcursorPosition = Camera.main.ScreenToWorldPoint(TimingGridcursorPoint) + TimelineOffset;
        Debug.Log(cursorPosition.x);
        
        
        //bound grid and timeline
        if (cursorPosition.x > -1.7 && cursorPosition.x < 10.84)
        {
            transform.position = cursorPosition;
            TimingGrid.transform.position = TimingGridcursorPosition;
        }
        //else snap to max or min
        else if (cursorPosition.x <= -1.7f)
        {
            transform.position = new Vector3(-1.7f, cursorPosition.y, cursorPosition.z);
            TimingGrid.transform.position = new Vector3(-1.7f, TimingGridcursorPosition.y, TimingGridcursorPosition.z);
        }
        else if (cursorPosition.x >= 10.84f)
        {
            transform.position = new Vector3(10.84f, cursorPosition.y, cursorPosition.z);
            TimingGrid.transform.position = new Vector3(10.84f, TimingGridcursorPosition.y, TimingGridcursorPosition.z);
        }
    }
}
