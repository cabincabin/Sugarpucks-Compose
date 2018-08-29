using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddMesures : MonoBehaviour
{
    //this is added to the mesure button
    public int MesuresToAdd;
    //calls the timeline class
    public TimelinePositioning timeline;

    private void OnMouseUpAsButton()
    {
        //adds the number of mesures specified to the timeline
        for (int i = 0; i < MesuresToAdd; i++)
        {
            //by calling the append mesure method, which adds a single mesure
            timeline.AppendMesure();
        }
    }
}