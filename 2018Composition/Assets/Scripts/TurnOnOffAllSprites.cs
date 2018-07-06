using System.Collections.Generic;
using UnityEngine;

    public class TurnOnOffAllSprites : MonoBehaviour
    {
        private bool OnOff;
        public List<GameObject> turnSpriteOff;
        
        private void OnMouseUpAsButton()
        {
            if (OnOff)
            {
                foreach (var spriteGameObj in turnSpriteOff)
                {
                    spriteGameObj.gameObject.GetComponent<SpriteRenderer>().enabled = true;  
                }
                OnOff = false;

            }
            else
            {
                foreach (var spriteGameObj in turnSpriteOff)
                {
                    spriteGameObj.gameObject.GetComponent<SpriteRenderer>().enabled = false;  
                }
                OnOff = true;
            }
        }

      
    }