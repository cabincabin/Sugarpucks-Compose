using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayableSprite : MonoBehaviour
 {
     //0-11 for a1-g#1
     //12-24 for a2-g#2
     public int PitchNumber;
     
     float clicked = 0;
     float clicktime = 0;
     float clickdelay = .25f;


     public void ChangePitchUpDown()
     {
         if (PitchNumber / 12f < 1)
         {
             PitchNumber = PitchNumber + 12;
             transform.eulerAngles = new Vector3(0, 0, 270);
         }
         else
         {
             PitchNumber = PitchNumber - 12;
             transform.eulerAngles = new Vector3(0, 0, 0);
         }
     }
     
     private void OnMouseDown()
     { 
         //allows the puck to move from gridspace to gridspace
         name = "Move";
         
     }

     private void OnMouseUp()
     {
         //allows the puck to lock to the given grid the puck is hovering above
         name = "PlayPuck";
         
         bool keepInPos = name.Equals("SugarStick");

         if (clicked > 1 || Time.time - clicktime > clickdelay) 
             clicked = 0;
         Debug.Log("ResetClickClick");
         Debug.Log(Time.time - clicktime);
         if (clicked == 0)
         {
             Debug.Log("Click");
             clicked++;
             clicktime = Time.time;
             if (keepInPos)
                 name = "SugarStick";
         }
         //if doubleclicked, change note up an octive
         else if (clicked == 1 && Time.time - clicktime < clickdelay)
         {
             Debug.Log("ClickClick");
             clicked = 0;
             ChangePitchUpDown();
         }
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