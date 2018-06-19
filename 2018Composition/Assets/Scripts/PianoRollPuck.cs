using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class PianoRollPuck : MonoBehaviour{
    public GameObject PlaySprite;
    public AudioClip pitch;
    public Key Keychanger;
    
    public TimelinePositioning timeline;
    private bool CanWiggle;
    private bool wiggleRight;
    private TimingGrid lastGrid;
    private int SpritesInLastGrid= 0;
     
    private void Update()
    {
        if (CanWiggle)
        {
            wiggle();
        }
        if(Keychanger.hasKey)
            RecomendChord(); 
    }

    //if the puck should wiggle, wiggles the puck right and left at a medium speed to "suggest" the puck
    private void wiggle()
    {
        if (!wiggleRight){
            if(transform.rotation.eulerAngles.z < 45 || transform.rotation.eulerAngles.z > 300)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + 3);
            }
            else
            {
                wiggleRight = true;
            }
        }

        if (wiggleRight)
        {
            if (transform.rotation.eulerAngles.z > 315 || transform.rotation.eulerAngles.z < 60)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z - 3);
            }
            else
            {
                wiggleRight = false;
            }
        }
    }

    private void RecomendChord()
         {
             
             List<GameObject> GridsAsGameObj = timeline.TimingGrids;
             //for each grid
             TimingGrid CurrGrid = GridsAsGameObj[0].GetComponent<TimingGrid>();
             for(int beat = 0; beat < GridsAsGameObj.Count; beat++)
             {
                 //find if there are any pucks added to the beat
                 TimingGrid Grid = GridsAsGameObj[beat].GetComponent<TimingGrid>();
                 if (Grid.Sprites.Count>0)
                 {
                     CurrGrid = Grid;
                 }
             }

             //if there is a grid update
             if (CurrGrid.Sprites.Count>0 && (CurrGrid != lastGrid || SpritesInLastGrid != CurrGrid.Sprites.Count))
             {
                 CanWiggle = false;
                 transform.eulerAngles = new Vector3(0, 0, 0);
                 lastGrid = CurrGrid;
                 SpritesInLastGrid = CurrGrid.Sprites.Count;

                 //these should be moved to a helper method.
                 
                 //if there's only 1 note in the last grid
                 if (lastGrid.Sprites.Count == 1)
                 {
                     //find the notes in the key
                     int pitchNum = lastGrid.Sprites[0].GetComponent<PlayableSprite>().PitchNumber;
                                
                     int keyNumIndex = -1;
                     for (int index = 0; index < Keychanger.NumInKey.Count; index++)
                     {
                         if (Keychanger.NumInKey[index] == pitchNum)
                         {
                             keyNumIndex = index;
                         }
                     }
                     
                     
                     if ( keyNumIndex != -1 && Keychanger.NumInKey[(keyNumIndex + 2) % Keychanger.NumInKey.Count] ==
                         PlaySprite.GetComponent<PlayableSprite>().PitchNumber % 12)
                     {
                         CanWiggle = true;
                     }
                 }
                 
                 //if there are 2 notes in the grid
                 if (lastGrid.Sprites.Count == 2)
                 {
                     //find the notes in the key
                     int pitchNum1 = lastGrid.Sprites[0].GetComponent<PlayableSprite>().PitchNumber;
                     int pitchNum2 = lastGrid.Sprites[1].GetComponent<PlayableSprite>().PitchNumber;
                                
                     int keyNumIndex1 = -1;
                     int keyNumIndex2 = -1;
                     //find the notes in the key
                     for (int index = 0; index < Keychanger.NumInKey.Count; index++)
                     {
                         if (Keychanger.NumInKey[index] == pitchNum1)
                         {
                             keyNumIndex1 = index;
                         }
                         if (Keychanger.NumInKey[index] == pitchNum2)
                         {
                             keyNumIndex2 = index;
                         }
                     }
                     
                     Debug.Log(keyNumIndex1);
                     Debug.Log(keyNumIndex2);
                     
                     //find which note is the base of the triad, after assessing if the notes can be a triad
                     if (keyNumIndex1 != -1 && keyNumIndex2 != -1)
                     {
                         //assess if the first index is the base of the triad
                         if ((keyNumIndex1 + 2) % Keychanger.NumInKey.Count == keyNumIndex2)
                         {
                             //wiggle the last note in the triad
                             if (Keychanger.NumInKey[(keyNumIndex2 + 2) % Keychanger.NumInKey.Count] ==
                                 PlaySprite.GetComponent<PlayableSprite>().PitchNumber % 12)
                             {
                                 CanWiggle = true;
                             }
                         }
                         //assess if the second index is the base of the triad
                         if ((keyNumIndex2 + 2) % Keychanger.NumInKey.Count == keyNumIndex1)
                         {
                             //wiggle the last note in the triad
                             if (Keychanger.NumInKey[(keyNumIndex1 + 2) % Keychanger.NumInKey.Count] ==
                                 PlaySprite.GetComponent<PlayableSprite>().PitchNumber % 12)
                             {
                                 CanWiggle = true;
                             }
                         }
                     }
                 }
             }
         }
    
    void OnMouseDown()
    {
        Keychanger.chooseKey(PlaySprite.GetComponent<PlayableSprite>().PitchNumber%12);
        //Play the audio on click and generate the next sprite
        //bug where it takes 2 clicks to actually move the sprite
        Instantiate(PlaySprite, transform.position, Quaternion.identity);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.clip = pitch;
        audio.Play();
    }

    
}