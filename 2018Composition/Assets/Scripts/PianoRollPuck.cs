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
        Instantiate(Target, transform.position, Quaternion.identity);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.clip = pitch;
        audio.Play();
    }
    
    
}