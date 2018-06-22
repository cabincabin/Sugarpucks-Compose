using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Twinkle : MonoBehaviour
{
    //11 55 66 5 44 33 22 1

    public Key currKey;
    public List<GameObject> SpriteNotes;
    public TimelinePositioning timeline;
    public ClearGrid clear;
    private List<int> RelitiveNotes;

    private void Start()
    {
        //TwinkleTwinkle
        //get the notes in relitive notation, 0 indexed. -1 is a rest note
        RelitiveNotes = new List<int>{0, 0, 4, 4, 5, 5, 4, -1, 3, 3, 2, 2, 1, 1, 0, -1};
        //RelitiveNotes = new List<int>{2, 1, 0, 1, 2, 2, 2, -1, 1, 1, 1, -1, 2, 4, 4, -1, 2, 1, 0, 1, 2, 2, 2, -1, 2, 1, 2, 1, 0, -1};
    }

    private void OnMouseUpAsButton()
    {
        //if there is a key
        if (currKey.hasKey)
        {
            //clear the board
            clear.Clear();
            int timelinePosCounter = 0;
            //for each relitive note
            foreach (var relitiveIndex in RelitiveNotes)
            {
                //get the note 
                if (relitiveIndex != -1)
                {
                    //keep the song the same, so rase the note an octive if above the base key. 
                    //in twinkle, the song starts at the key and goes UP not down, no notes should be below the key note
                    if (currKey.NumInKey[relitiveIndex] < currKey.NumInKey[0])
                    {
                        addNoteToTimeline(currKey.NumInKey[relitiveIndex] + 12, timelinePosCounter);
                    }
                    else
                    {
                        addNoteToTimeline(currKey.NumInKey[relitiveIndex], timelinePosCounter);
                    }
                }
                timelinePosCounter++;
            }

            
        }
    }

    private void addNoteToTimeline(int NoteNum, int position)
    {
        List<GameObject> gridsAsGameObjects = timeline.TimingGrids;
        GameObject PlayToAdd = SpriteNotes[0];
        //get the right note and make a copy of it
        foreach (var PlayPuck in SpriteNotes)
        {
            if (PlayPuck.GetComponent<PlayableSprite>().PitchNumber%12 == NoteNum%12)
            {
                PlayToAdd = Instantiate(PlayPuck);
            }
        }
        //add the note to the grid
        PlayToAdd.name = "SugarStick";
        if(NoteNum>=12)
            PlayToAdd.GetComponent<PlayableSprite>().ChangePitchUpDown();
        if(position < gridsAsGameObjects.Count)
            gridsAsGameObjects[position].GetComponent<TimingGrid>().Sprites.Add(PlayToAdd.GetComponent<Collider2D>());
    }
}
