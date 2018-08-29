using System.Collections.Generic;
using UnityEngine;

    public class TurnOnOffAllSprites : MonoBehaviour
    {
        private bool OnOff;
        //list of objects to make visable/invisable
        public List<GameObject> turnSpriteOff;
        
        private void OnMouseUpAsButton()
        {
            //flip the variable every time
            if (OnOff)
            {
                //for all the objects
                foreach (var spriteGameObj in turnSpriteOff)
                {
                    //make them visable
                    spriteGameObj.gameObject.GetComponent<SpriteRenderer>().enabled = true;  
                }
                OnOff = false;

            }
            else
            {
                //for all objects in list
                foreach (var spriteGameObj in turnSpriteOff)
                {
                    //make them invisable
                    spriteGameObj.gameObject.GetComponent<SpriteRenderer>().enabled = false;  
                }
                OnOff = true;
            }
        }

      
    }