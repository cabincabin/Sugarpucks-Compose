using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

//each grid is a note to be added to the STEP cs file
public class TimingGrid : MonoBehaviour
{
        //list of up to 12 sprites to play on a given beat
        //do not add anything to this
        public List<Collider2D> Sprites;
        public int BeatNum = 0;

        void Start()
        {
                //init list
                Sprites = new List<Collider2D>();
        }

        private void Update()
        {
                float CurrXPos = transform.position.x;
                Collider2D[] collideObjects = Physics2D.OverlapAreaAll(new Vector2(CurrXPos-5.6f,3.9f), new Vector2(CurrXPos-4.14f, -4.75f));
                //detect and add any new sugar pucks in the column of the given grid/beat
                foreach (var other in collideObjects)
                {
                        //check and make sure that the puck is only grabbed when dropped onto the grid
                        //not while the user is moving it
                        if (other.gameObject.name=="PlayPuck" && !Sprites.Contains(other))
                        {
                                Sprites.Add(other);
                                other.name = "SugarStick";
                                //0 is default, 1 indexed
                                if (other.gameObject.GetComponents<ChangeSkin>().Length != 0)
                                        other.GetComponent<ChangeSkin>().changeSkin(BeatNum+1);
                        } 
                }

                //track the pucks so that they stay locked to the same gridspace until moved
                foreach (var puck in Sprites)
                {
                        if (puck == null)
                        {
                                Sprites.Remove(puck);
                                break;
                        }
                        else if (puck.name!="Move")
                        {
                           //if its a part of the gridspace
                           //move the pucks so they stay alligned with the gridspace
                                Vector3 BodyLocation = new Vector3(CurrXPos-4.86f, puck.transform.position.y, 7);
                                puck.transform.position = BodyLocation;   
                        }
             
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

