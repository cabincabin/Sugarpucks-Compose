using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayableSprite : MonoBehaviour
 {
     //1-12 for a1-g#1
     //12-24 for a2-g#2
     public int Number;
     

     private void OnMouseDown()
     {
         //allows the puck to move from gridspace to gridspace
         name = "Move";
         
     }

     private void OnMouseUp()
     {
         //allows the puck to lock to the given grid the puck is hovering above
         name = "PlayPuck";
     }


     //update the timeline position and move the TimingGrids by the relitive distance needed for transform
     void OnMouseDrag(){
        
         //update timeline position
         Vector3 cursorPoint = new Vector3(Input.mousePosition.x, transform.position.y, transform.position.z);
         Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
         cursorPosition=new Vector3(cursorPosition.x, transform.position.y, transform.position.z);
         transform.position = cursorPosition;
     }
     
     
 }