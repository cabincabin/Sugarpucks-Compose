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
     private Vector3 CurrScale = new Vector3(-.3f, .25f, 1f);
     
     float clicked = 0;
     float clicktime = 0;
     float clickdelay = .25f;

    //change the pitch up, or down based on the current pitch, turning the puck sideways for indication 2nd octive

     public void ChangePitchUpDown()
     {
         if (PitchNumber / 12f < 1)
         {
             PitchNumber = PitchNumber + 12;
             CurrScale = new Vector3(-.25f, .3f, 1f);
             transform.localScale = CurrScale;
             transform.eulerAngles = new Vector3(0, 0, 292.5f);
             
         }
         else
         {
             PitchNumber = PitchNumber - 12;
             CurrScale = new Vector3(-.3f, .25f, 1f);
             transform.localScale = CurrScale;
             transform.eulerAngles = new Vector3(0, 0, 0);
         }
     }
     
     private void OnMouseDown()
     { 
         //allows the puck to move from gridspace to gridspace
         name = "Move";
         transform.localScale = new Vector3(-.35f, .3f, 1f);
         
     }

     private void OnMouseUp()
     {
         //allows the puck to lock to the given grid the puck is hovering above
         name = "PlayPuck";
         
         bool keepInPos = name.Equals("SugarStick");

         //check if the puck has been double clicked.
         //if so, change the octive of the puck
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
         transform.localScale = CurrScale;
         
         if (transform.position.x < -5.6f)
         {
             GetComponent<SpriteRenderer>().enabled = false;
             GetComponent<Collider2D>().enabled = false;
             Destroy(this);
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