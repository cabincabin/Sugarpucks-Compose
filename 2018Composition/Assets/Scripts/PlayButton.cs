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
            //CreateAndPlaySequence.stepCount = GridsAsGameObj.Count;
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
        
