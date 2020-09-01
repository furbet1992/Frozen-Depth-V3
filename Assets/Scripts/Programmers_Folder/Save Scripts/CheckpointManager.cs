﻿/*
    File name: CheckpointManager.cs
    Author:    Luke Lazzaro
    Summary: Handles the current checkpoint counter and updates checkpoints
    Creation Date: 10/08/2020
    Last Modified: 31/08/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    // The current checkpoint counter. The current value represents the last checkpoint you touched, not how many you've passed.
    // All previous checkpoints will be disabled when updated.
    public static int checkpointCounter = 0;
    public static GameObject currentCheckpoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void UpdateCheckpoints()
    {
        foreach (Transform child in transform)
        {
            // Update checkpoints based on checkpoint counter
            Checkpoint current = child.gameObject.GetComponent<Checkpoint>();

            if (current.checkpointNumber < checkpointCounter)
                current.gameObject.SetActive(false);
            else
                current.gameObject.SetActive(true);
        }
    }
}
