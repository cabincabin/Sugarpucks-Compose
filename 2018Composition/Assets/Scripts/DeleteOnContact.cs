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
        if (destroyAll)
        {
            Collider2D[] collideObjects = Physics2D.OverlapAreaAll(new Vector2(-10f,4.2f), new Vector2(-5.4f, -5f) );
            //detect when the sprite is moved out of the grid and remove it from the list
            foreach (var other in collideObjects)
            {
                if (other.gameObject.GetComponents<PlayableSprite>().Length != 0 && !other.gameObject.name.Equals("SugarStick"))
                {
                    other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Destroy(other);
                }
            }
           

            destroyAll = false;
            Debug.Log("WHYAMIHERE?");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //detect when the sprite is moved out of the grid and remove it from the list
        if (other.gameObject.name.Equals("Move"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other);
        }
    }
    
}
