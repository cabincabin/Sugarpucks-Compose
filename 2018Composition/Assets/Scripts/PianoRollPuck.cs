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
    private int SpritesInLastGrid=0;
    public float ZeroPos;
    public float TopPos;
    public List<GameObject> VisualSteps;
    
    private void Start()
    {
        if (VisualSteps != null)
        {
            foreach (var step in VisualSteps)
            {
                step.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    
    private void Update()
    {
        if (Keychanger != null)
        {
            //if the puck is the next one in the triad, the puck shoul wiggle
            if (CanWiggle)
            {
                wiggle();
            }

            //if there is a key that the song is currently in, use that key to create the chord
            if (Keychanger.hasKey)
                RecomendChord();
        }
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

    //this is a long one and should be broken into helpers
    //convert whats between the ///////// to helper
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
                     //get the last grid with anything in it 
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
                     
                     //find the note in the key that the current sprite is.
                     //if the sprite is A in the key of A, the index would be 0
                     //B in the key of A would be 1... ect
                     int keyNumIndex = -1;
                     for (int index = 0; index < Keychanger.NumInKey.Count; index++)
                     {
                         if (Keychanger.NumInKey[index] == pitchNum)
                         {
                             keyNumIndex = index;
                         }
                     }
                     
                     //if the current puck in the last gridspace is in the key AND this sugar puck is the next note in the triad, wiggle ths puck
                     if ( keyNumIndex != -1 && Keychanger.NumInKey[(keyNumIndex + 2) % Keychanger.NumInKey.Count] ==
                         PlaySprite.GetComponent<PlayableSprite>().PitchNumber % 12)
                     {
                         CanWiggle = true;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                         Keychanger.ClearAllPuckChordLines();
                         
                         if ( Keychanger.NumInKey[(keyNumIndex) % Keychanger.NumInKey.Count] >  Keychanger.NumInKey[(keyNumIndex + 2) % Keychanger.NumInKey.Count])
                         {
                             if ((keyNumIndex + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex + 2) % Keychanger.NumInKey.Count == 1 || (keyNumIndex + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex + 2) % Keychanger.NumInKey.Count == 0)
                             {
                                 GameObject Step = Instantiate(VisualSteps[2]);
                                 VisualSteps.Add(Step);
                                 Step.GetComponent<SpriteRenderer>().enabled = true;
                                 Vector3 StepPos = Step.transform.position;
                                 Step.transform.position = new Vector3(StepPos.x + 1f,
                                     StepPos.y - (TopPos - transform.position.y), StepPos.z);
                             }
                             else
                             {
                                 GameObject Step = Instantiate(VisualSteps[3]);
                                 VisualSteps.Add(Step);
                                 Step.GetComponent<SpriteRenderer>().enabled = true;
                                 Vector3 StepPos = Step.transform.position;
                                 Step.transform.position = new Vector3(StepPos.x + 1f,
                                     StepPos.y - (TopPos - transform.position.y), StepPos.z);
                             }
                         }
                         else if ((keyNumIndex + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex + 2) % Keychanger.NumInKey.Count == 1 ||(keyNumIndex + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex + 2) % Keychanger.NumInKey.Count == 0)
                         {
                             GameObject Step = Instantiate(VisualSteps[0]);
                             VisualSteps.Add(Step);
                             Step.GetComponent<SpriteRenderer>().enabled = true;
                             Vector3 StepPos = Step.transform.position;
                             Step.transform.position = new Vector3(StepPos.x + 1f,
                                 StepPos.y + (transform.position.y -
                                              ZeroPos), StepPos.z);
                         }
                         else
                         {
                             GameObject Step = Instantiate(VisualSteps[1]);
                             VisualSteps.Add(Step);
                             Step.GetComponent<SpriteRenderer>().enabled = true;
                             Vector3 StepPos = Step.transform.position;
                             Step.transform.position = new Vector3(StepPos.x + 1f,
                                 StepPos.y + (transform.position.y -
                                              ZeroPos), StepPos.z);  
                         }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                     //find the notes in the key from the sugar pucks in the last grid.
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
                                 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                             
                             if ( Keychanger.NumInKey[(keyNumIndex2) % Keychanger.NumInKey.Count] >  Keychanger.NumInKey[(keyNumIndex2 + 2) % Keychanger.NumInKey.Count])
                             {
                                 if ((keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 1 || (keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 0)
                                 {
                                     GameObject Step = Instantiate(VisualSteps[2]);
                                     VisualSteps.Add(Step);
                                     Step.GetComponent<SpriteRenderer>().enabled = true;
                                     Vector3 StepPos = Step.transform.position;
                                     Step.transform.position = new Vector3(StepPos.x + 1f,
                                         StepPos.y - (TopPos - transform.position.y), StepPos.z);
                                 }
                                 else
                                 {
                                     GameObject Step = Instantiate(VisualSteps[3]);
                                     VisualSteps.Add(Step);
                                     Step.GetComponent<SpriteRenderer>().enabled = true;
                                     Vector3 StepPos = Step.transform.position;
                                     Step.transform.position = new Vector3(StepPos.x + 1f,
                                         StepPos.y - (TopPos - transform.position.y), StepPos.z);
                                 }
                             }
                             else if ((keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 1 ||(keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex2 + 2) % Keychanger.NumInKey.Count == 0)
                             {
                                 GameObject Step = Instantiate(VisualSteps[0]);
                                 VisualSteps.Add(Step);
                                 Step.GetComponent<SpriteRenderer>().enabled = true;
                                 Vector3 StepPos = Step.transform.position;
                                 Step.transform.position = new Vector3(StepPos.x + 1f,
                                     StepPos.y + (transform.position.y -
                                                  ZeroPos), StepPos.z);
                             }
                             else
                             {
                                 GameObject Step = Instantiate(VisualSteps[1]);
                                 VisualSteps.Add(Step);
                                 Step.GetComponent<SpriteRenderer>().enabled = true;
                                 Vector3 StepPos = Step.transform.position;
                                 Step.transform.position = new Vector3(StepPos.x + 1f,
                                     StepPos.y + (transform.position.y -
                                                  ZeroPos), StepPos.z);  
                             }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                                 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                 if ( Keychanger.NumInKey[(keyNumIndex1) % Keychanger.NumInKey.Count] >  Keychanger.NumInKey[(keyNumIndex1 + 2) % Keychanger.NumInKey.Count])
                                 {
                                     if ((keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 1 || (keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 0)
                                     {
                                         GameObject Step = Instantiate(VisualSteps[2]);
                                         VisualSteps.Add(Step);
                                         Step.GetComponent<SpriteRenderer>().enabled = true;
                                         Vector3 StepPos = Step.transform.position;
                                         Step.transform.position = new Vector3(StepPos.x + 1f,
                                             StepPos.y - (TopPos - transform.position.y), StepPos.z);
                                     }
                                     else
                                     {
                                         GameObject Step = Instantiate(VisualSteps[3]);
                                         VisualSteps.Add(Step);
                                         Step.GetComponent<SpriteRenderer>().enabled = true;
                                         Vector3 StepPos = Step.transform.position;
                                         Step.transform.position = new Vector3(StepPos.x + 1f,
                                             StepPos.y - (TopPos - transform.position.y), StepPos.z);
                                     }
                                 }
                                 else if ((keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 4 || (keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 1 ||(keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 3 ||(keyNumIndex1 + 2) % Keychanger.NumInKey.Count == 0)
                                 {
                                     GameObject Step = Instantiate(VisualSteps[0]);
                                     VisualSteps.Add(Step);
                                     Step.GetComponent<SpriteRenderer>().enabled = true;
                                     Vector3 StepPos = Step.transform.position;
                                     Step.transform.position = new Vector3(StepPos.x + 1f,
                                         StepPos.y + (transform.position.y -
                                                      ZeroPos), StepPos.z);
                                 }
                                 else
                                 {
                                     GameObject Step = Instantiate(VisualSteps[1]);
                                     VisualSteps.Add(Step);
                                     Step.GetComponent<SpriteRenderer>().enabled = true;
                                     Vector3 StepPos = Step.transform.position;
                                     Step.transform.position = new Vector3(StepPos.x + 1f,
                                         StepPos.y + (transform.position.y -
                                                      ZeroPos), StepPos.z);  
                                 }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                             }
                         }
                     }
                 }
             }
         }
    
    void OnMouseDown()
    {
        //on the event that a new key can be chosen, make the key this current sugar puck if clicked.
        if (Keychanger != null)
            Keychanger.chooseKey(PlaySprite.GetComponent<PlayableSprite>().PitchNumber%12);
        //Play the audio on click and generate the next sprite
        //bug where it takes 2 clicks to actually move the sprite
        Instantiate(PlaySprite, transform.position, Quaternion.identity);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = pitch;
        audio.Play();
    }


    
}