using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteOnContact : MonoBehaviour
{
    public bool destroyAll=false;

    private void Update()
    {
        //if a new key was chosen, or the public boolean destroy all was flipped
        if (destroyAll)
        {
            //find all objects on the piano section
            Collider2D[] collideObjects = Physics2D.OverlapAreaAll(new Vector2(-10f,4.2f), new Vector2(-5.4f, -5f) );
            //for all objects within the given's object's hitbox
            foreach (var other in collideObjects)
            {
                //if the object is an extranious note puck
               if (other.gameObject.GetComponents<PlayableSprite>().Length != 0 && !other.gameObject.name.Equals("SugarStick"))
                {
                    //delete the puck
                    other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    other.gameObject.GetComponent<Collider2D>().enabled = false;
                    Destroy(other);
                }
            }
            destroyAll = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //detect when the sprite is moved into the object from outside the object
        if (other.gameObject.name.Equals("Move"))
        {
            //if its moved in, delete it
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other);
        }
    }
    
}
