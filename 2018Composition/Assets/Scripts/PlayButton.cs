using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayButton : MonoBehaviour
{
    public TimelinePositioning timeline;
    public Stepp CreateAndPlaySequence;
    private bool isPlay;
    public Sprite play;
    public Sprite stop;
    public GameObject PlayPos;
   

    void Start()
    {
        //hide the timing bar to start
        PlayPos.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    private void Update()
    {
        if (isPlay)
        {
            
            PlayPos.GetComponent<SpriteRenderer>().enabled = true;
            if (CreateAndPlaySequence.currentStep<11)
            {
                timeline.TimelinePosTo(0);
                PlayPos.transform.position = new Vector3((CreateAndPlaySequence.currentStep)*timeline.lenOfTimelineSeg,PlayPos.transform.position.y,PlayPos.transform.position.z);
            }
            else
            {
                timeline.TimelinePosTo(CreateAndPlaySequence.currentStep+1);
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        //invert button every click, starting or stoping the subroutine
        isPlay = !isPlay;
        SpriteRenderer spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
        if (isPlay)
        {
            
            //wipe the current puck list
            CreateAndPlaySequence.ClearAllPitches();
            //Get all of the grids
            List<GameObject> GridsAsGameObj = timeline.TimingGrids;
            //for each grid
            CreateAndPlaySequence.stepCount = GridsAsGameObj.Count;
            CreateAndPlaySequence.Awake();
            for(int beat = 0; beat < GridsAsGameObj.Count; beat++)
            {
                //find if there are any pucks added to the beat
                TimingGrid Grid = GridsAsGameObj[beat].GetComponent<TimingGrid>();
               
                for(int PuckIndex = Grid.Sprites.Count-1; PuckIndex >= 0; PuckIndex--)
                {
                    //get the puck's note and add the note to the corrisponding note in the sequence.
                    if (Grid.Sprites[PuckIndex].GetComponents<PlayableSprite>().Length != 0)
                    {
                        PlayableSprite SugarPuck = Grid.Sprites[PuckIndex].GetComponent<PlayableSprite>();
                        CreateAndPlaySequence.AddPitchAtStep(SugarPuck.PitchNumber,beat);
                    }
                    //if there's some sort of puck existance error, remove the error
                    else
                    {
                        Grid.Sprites[PuckIndex].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        Grid.Sprites[PuckIndex].GetComponent<Collider2D>().enabled = false;
                        Destroy(Grid.Sprites[PuckIndex]);
                        Grid.Sprites.RemoveAt(PuckIndex);
                    }

                    
                }
            }
            CreateAndPlaySequence.StartSequencer();
            spriteRenderer.sprite = stop;
        }
        //Stop Stepp
        if (!isPlay)
        {
            CreateAndPlaySequence.StopSequencer();
            spriteRenderer.sprite = play;
        }
        
    }
}
        
