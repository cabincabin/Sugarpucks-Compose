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

    private void OnMouseUpAsButton()
    {
        //invert button every click, starting or stoping the subroutine
        isPlay = !isPlay;

        if (isPlay)
        {
            //wipe the current puck list
            CreateAndPlaySequence.ClearAllPitches();
            //Get all of the grids
            List<GameObject> GridsAsGameObj = timeline.TimingGrids;
            //for each grid
            for(int beat = 0; beat < GridsAsGameObj.Count; beat++)
            {
                //find if there are any pucks added to the beat
                TimingGrid Grid = GridsAsGameObj[beat].GetComponent<TimingGrid>();
                foreach (var puck in Grid.Sprites)
                {
                    //get the puck's note and add the note to the corrisponding note in the sequence.
                    PlayableSprite SugarPuck = puck.GetComponent<PlayableSprite>();
                    CreateAndPlaySequence.AddPitchAtStep(SugarPuck.PitchNumber,beat);
                }
            }
            CreateAndPlaySequence.StartSequencer();
        }
        //Stop Stepp
        if (!isPlay)
        {
            CreateAndPlaySequence.StopSequencer();
        }
        
    }
}
        
