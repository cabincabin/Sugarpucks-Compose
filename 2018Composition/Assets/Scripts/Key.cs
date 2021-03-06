﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Key : MonoBehaviour
{
    //should be 12, one for each piano puck
    //should be in order from a to g# with a being the first number
    //this could be confirmed using the numbers of each sprite if this is too much effort
    public List<GameObject> PianoPucks;
    //0 should be word key
    //1 - 12 should be a through g#
    public List<Sprite> KeyIcon;
    public DeleteOnContact PianoContact;

    //do not touch
    public bool hasKey;
    //piano pucks default location
    private float defaultx = -8;
    private bool CanChooseKey;
    //do not touch
    public List<int> NumInKey;
    public List<GameObject> VisualSteps;
    //if this is extended further, visual steps will cause problems with garbage collection, as currently, each time
    //a new key is chosen, none of the previous step sprites are deleated, they are just turned off, and new ones are created
    //whoops

    private void Start()
    {
        //this is for the create a key section
        if (VisualSteps.Count != 0)
        {
            foreach (var step in VisualSteps)
            {
                //make all the key steps invisable
                step.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    //when selected allow for key change
    private void OnMouseUpAsButton()
    {
        //Let the next note chosen determine the key
        CanChooseKey = true;
        
        //this should be moved to change skin
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        //render word key
        spriteRenderer.sprite = KeyIcon[0];
    }

    //allows piano puck to choose key
    public void chooseKey(int KeyNum)
    {
        //if a key can be and is chosen
        if (CanChooseKey)
        {
            //clears all line pointers for chords when a key is chosen in the LESSONS only
            ClearAllPuckChordLines();

            //Hide all the step counts for the Key Steps Only 
            if (VisualSteps.Count != 0)
            {
                foreach (var step in VisualSteps)
                {
                    step.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            //show that a key has been choosen
            hasKey = true;
            NumInKey = new List<int>
            {
                KeyNum, (KeyNum+2)%12, (KeyNum+4)%12, (KeyNum+5)%12, (KeyNum+7)%12, (KeyNum+9)%12, (KeyNum+11)%12
            };

            //reset position of all pucks
            foreach (var Puck in PianoPucks)
            {
                Puck.transform.position = new Vector3(defaultx,  Puck.transform.position.y,  Puck.transform.position.z);
                Puck.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            }

            for(int numIndex = 0; numIndex<NumInKey.Count; numIndex++)
            {
                PianoPucks[NumInKey[numIndex]].transform.position = new Vector3(defaultx + 1f,  PianoPucks[NumInKey[numIndex]].transform.position.y,  PianoPucks[NumInKey[numIndex]].transform.position.z);
                PianoPucks[NumInKey[numIndex]].GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        
                 if(VisualSteps.Count != 0){
                    if (NumInKey[numIndex] > NumInKey[(numIndex + 1) % NumInKey.Count])
                    {
                        if (numIndex == 2 || numIndex == 6)
                        {
                            GameObject Step = Instantiate(VisualSteps[2]);
                            VisualSteps.Add(Step);
                            Step.GetComponent<SpriteRenderer>().enabled = true;
                            Vector3 StepPos = Step.transform.position;
                            Step.transform.position = new Vector3(StepPos.x + 1f,
                                StepPos.y - (PianoPucks[PianoPucks.Count - 1].transform.position.y -
                                             PianoPucks[NumInKey[numIndex]].transform.position.y), StepPos.z);
                        }
                        else
                        {
                            GameObject Step = Instantiate(VisualSteps[3]);
                            VisualSteps.Add(Step);
                            Step.GetComponent<SpriteRenderer>().enabled = true;
                            Vector3 StepPos = Step.transform.position;
                            Step.transform.position = new Vector3(StepPos.x + 1f,
                                StepPos.y - (PianoPucks[PianoPucks.Count - 1].transform.position.y -
                                             PianoPucks[NumInKey[numIndex]].transform.position.y), StepPos.z);
                        }
                    }
                    else if (numIndex == 2 || numIndex == 6)
                    {
                        GameObject Step = Instantiate(VisualSteps[0]);
                        VisualSteps.Add(Step);
                        Step.GetComponent<SpriteRenderer>().enabled = true;
                        Vector3 StepPos = Step.transform.position;
                        Step.transform.position = new Vector3(StepPos.x + 1f,
                            StepPos.y + (PianoPucks[NumInKey[numIndex]].transform.position.y -
                                         PianoPucks[0].transform.position.y), StepPos.z);
                    }
                    else
                    {
                        GameObject Step = Instantiate(VisualSteps[1]);
                        VisualSteps.Add(Step);
                        Step.GetComponent<SpriteRenderer>().enabled = true;
                        Vector3 StepPos = Step.transform.position;
                        Step.transform.position = new Vector3(StepPos.x + 1f,
                            StepPos.y + (PianoPucks[NumInKey[numIndex]].transform.position.y -
                                         PianoPucks[0].transform.position.y), StepPos.z);
                    }
                }
                if (numIndex == 0)
                 { GameObject Step = Instantiate(VisualSteps[4]);
                     VisualSteps.Add(Step);
                     Step.GetComponent<SpriteRenderer>().enabled = true;
                     Vector3 StepPos = Step.transform.position;
                     Step.transform.position = new Vector3(StepPos.x + 1f,
                         StepPos.y + (PianoPucks[NumInKey[numIndex]].transform.position.y -
                                      PianoPucks[0].transform.position.y), StepPos.z);
                     
                 }
            }
            //shift first puck over
           
            CanChooseKey = false;
            
            SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
            //render keyname
            spriteRenderer.sprite = KeyIcon[KeyNum+1];
            PianoContact.destroyAll = true;
        }
    }

    public void ClearAllPuckChordLines()
    {
        foreach (var Puck in PianoPucks)
        {
            foreach (var Puckline in Puck.GetComponent<PianoRollPuck>().VisualSteps)
            {
                //makes all chord indicators, for the lesson, invisable
                Puckline.GetComponent<SpriteRenderer>().enabled = false;
            }

                
        }
    }
}