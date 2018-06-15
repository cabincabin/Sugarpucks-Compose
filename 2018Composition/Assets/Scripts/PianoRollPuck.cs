using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class PianoRollPuck : MonoBehaviour{
    public GameObject Target;
    public AudioClip pitch;
    
    void OnMouseDown()
    {
        //Play the audio on click and generate the next sprite
        //bug where it takes 2 clicks to actually move the sprite
        Instantiate(Target, transform.position, Quaternion.identity);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.clip = pitch;
        audio.Play();
    }
    
    
}