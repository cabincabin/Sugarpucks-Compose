using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimelinePositioning : MonoBehaviour{

    //When the timeline is clicked and dragged, move it along the top of the timeline
    void OnMouseDrag(){
        //transform mouse position to world position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          //if the mose is within the bounds of the timeline
        if (mousePos.x > -1.7 && mousePos.x < 10.84)
        {
            transform.position = new Vector3(mousePos.x, transform.position.y, transform.position.z);
        }
        //else snap to max or min
        else if (mousePos.x <= -1.7)
        {
            transform.position.x = new Vector3(1.7f, transform.position.y, transform.position.z);
        }
        else if (mousePos.x >= 10.84)
        {
            transform.position.x = new Vector3(10.84f, transform.position.y, transform.position.z);
        }
    }
}
