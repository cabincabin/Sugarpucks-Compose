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
        RelitiveNotes = new List<int>{0, 0, 4, 4, 5, 5, 4, -1, 3, 3, 2, 2, 1, 1, 0, -1};
    }

    private void OnMouseUpAsButton()
    {
        if (currKey.hasKey)
        {
            clear.Clear();
            int timelinePosCounter = 0;
            foreach (var relitiveIndex in RelitiveNotes)
            {
                if (relitiveIndex != -1)
                {
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
        foreach (var PlayPuck in SpriteNotes)
        {
            if (PlayPuck.GetComponent<PlayableSprite>().PitchNumber%12 == NoteNum%12)
            {
                PlayToAdd = Instantiate(PlayPuck);
            }
        }
        PlayToAdd.name = "SugarStick";
        if(NoteNum>=12)
            PlayToAdd.GetComponent<PlayableSprite>().ChangePitchUpDown();
        
        if(position < gridsAsGameObjects.Count)
            gridsAsGameObjects[position].GetComponent<TimingGrid>().Sprites.Add(PlayToAdd.GetComponent<Collider2D>());
    }
}
