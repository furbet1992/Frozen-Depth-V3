﻿/*
    File name: Antidote.cs
    Author:    Luke Lazzaro
    Summary: Collectable that increases total every time one is picked up
    Creation Date: 5/10/2020
    Last Modified: 5/10/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Antidote : MonoBehaviour
{
    [HideInInspector]
    public static int antidoteTotal = 0;

    [SerializeField]
    private int dosageAmount = 0;

    [SerializeField]
    public Text blueAntidote;
    public Text redAntidote;
    public Text yellowAntidote;


    public void Update()
    {
        yellowAntidote.text = "Lives Saved:  " + antidoteTotal.ToString(); 
    }
    public void Collect()
    {
        antidoteTotal += dosageAmount;
        Destroy(gameObject);
    }
}
