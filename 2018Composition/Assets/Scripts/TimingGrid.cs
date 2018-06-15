using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimingGrid : MonoBehaviour
{

        private List<Collider2D> Sprites;

        void Start()
        {
                Sprites= new List<Collider2D>();
        }

        private void Update()
        {
                foreach (var puck in Sprites)
                {
                        if (puck.name!="Move")
                        {
                                Vector3 BodyLocation = new Vector3(transform.position.x-4.86f, puck.transform.position.y, 7);
                                puck.transform.position = BodyLocation;   
                        }
                          
                }
                
        }

        private void OnTriggerStay2D(Collider2D other)
        {
                if (other.gameObject.name=="PlayPuck" && !Sprites.Contains(other))
                {
                        Sprites.Add(other);
                        other.name = "SugarStick";
                }
               
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
   
                if (other.gameObject.name=="Move" && Sprites.Contains(other))
                {
                        Sprites.Remove(other);
                }
               
        }

      
}
