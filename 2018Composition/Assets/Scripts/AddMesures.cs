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
    public int MesuresToAdd;
    public TimelinePositioning timeline;

    private void OnMouseUpAsButton()
    {
        for (int i = 0; i < MesuresToAdd; i++)
        {
            timeline.AppendMesure();
        }
    }
}