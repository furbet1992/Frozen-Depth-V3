﻿/*
    File name: Subtitles.cs
    Author:    Luke Lazzaro
    Summary: Controls how the subtitles appear and change
    Creation Date: 27/07/2020
    Last Modified: 27/07/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Add subtitle sequences, so one subtitle will lead into another subtitle if needed.

public class Subtitles : MonoBehaviour
{
    [SerializeField] private float secondsUntilDisappear = 3;

    private float timeLeft = 0;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.enabled = false;
    }

    private void Update()
    {
        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;
        else
            text.enabled = false;
    }

    public void UpdateSubtitles(string newText)
    {
        text.enabled = true;
        text.text = newText;
        timeLeft = secondsUntilDisappear;
    }
}
