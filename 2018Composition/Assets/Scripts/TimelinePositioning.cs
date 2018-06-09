using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimelinePositioning : MonoBehaviour{

    private Vector3 screenPoint;
    private Vector3 offset;
		
    void Start () {
        Debug.Log("AHH");
    }
 
    void Update () {
    }
    
    void OnMouseDown()
    {
        Debug.Log("Here");
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z));
    }
    
    void OnMouseDrag(){

        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        Debug.Log(cursorPosition.x);
        
        if (cursorPosition.x > -1.7 && cursorPosition.x < 10.84)
        {
            transform.position = cursorPosition;
        }
        //else snap to max or min
        else if (cursorPosition.x <= -1.7f)
        {
            transform.position = new Vector3(-1.7f, cursorPosition.y, cursorPosition.z);
        }
        else if (cursorPosition.x >= 10.84f)
        {
            transform.position = new Vector3(10.84f, cursorPosition.y, cursorPosition.z);
        }
    }
}
