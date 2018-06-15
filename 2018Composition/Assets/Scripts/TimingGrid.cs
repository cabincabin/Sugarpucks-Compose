using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//each grid is a note to be added to the STEP cs file
public class TimingGrid : MonoBehaviour
{
        //list of up to 12 sprites to play on a given beat
        //do not add anything to this
        public List<Collider2D> Sprites;

        void Start()
        {
                //init list
                Sprites = new List<Collider2D>();
        }

        private void Update()
        {
                //track the pucks so that they stay locked to the same gridspace until moved
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
                //detect and add any new sugar pucks in the column of the given grid/beat
                if (other.gameObject.name=="PlayPuck" && !Sprites.Contains(other))
                {
                        Sprites.Add(other);
                        other.name = "SugarStick";
                }
               
        }
        
        
        private void OnTriggerExit2D(Collider2D other)
        {
                //detect when the sprite is moved out of the grid and remove it from the list
                if (other.gameObject.name=="Move" && Sprites.Contains(other))
                {
                        Sprites.Remove(other);
                }
               
        }

      
}
