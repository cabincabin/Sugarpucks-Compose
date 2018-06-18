using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class PianoRollPuck : MonoBehaviour{
    public GameObject PlaySprite;
    public AudioClip pitch;
    public Key Keychanger;
    
    public TimelinePositioning timeline;
    private bool wiggle; 
     
//    private void Update()
//    {
//        RecomendChord(); 
//    }
//
//    private void RecomendChord()
//         {
//             List<GameObject> GridsAsGameObj = timeline.TimingGrids;
//             //for each grid
//             TimingGrid lastGrid = GridsAsGameObj[0].GetComponent<TimingGrid>();;
//             for(int beat = 0; beat < GridsAsGameObj.Count; beat++)
//             {
//                 //find if there are any pucks added to the beat
//                 TimingGrid Grid = GridsAsGameObj[beat].GetComponent<TimingGrid>();
//                 if (Grid.Sprites.Count>0)
//                 {
//                     lastGrid = Grid;
//                 }
//             }
//     
//             if (lastGrid.Sprites.Count==1)
//             {
//                 int pitchNum = lastGrid.Sprites[0].GetComponent<PlayableSprite>().PitchNumber
//                 if (pitchNum == PlaySprite.GetComponent<PlayableSprite>().PitchNumber)
//             }
//         }
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //detect when the sprite is moved out of the grid and remove it from the list
        if (other.gameObject.name=="Move")
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other);
        }
    }
}