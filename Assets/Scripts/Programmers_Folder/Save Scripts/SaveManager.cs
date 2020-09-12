﻿/*
    File name: SaveManager.cs
    Author:    Luke Lazzaro
    Summary: Handles saving and loading of game data
    Creation Date: 3/08/2020
    Last Modified: 8/09/2020
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveGame(GameObject player)
    {
        SaveData data = new SaveData();
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;
        data.playerPosZ = player.transform.position.z;
        data.toolFuel = player.GetComponent<Tool>().toolFuel;
        data.currentCheckpoint = CheckpointManager.checkpointCounter;
        data.keys = KeyManager.Instance.keys;
        data.artifactIds = ArtifactManager.artifactIds;
        // save terrain mesh here

        PrintSaveData(data);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedata");
        bf.Serialize(file, data);
        file.Close();
    }

    // Call this method in LateUpdate, otherwise the position will get overwritten by other movement logic
    public static void LoadGame(GameObject player)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedata", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            player.GetComponent<Tool>().toolFuel = data.toolFuel;
            CheckpointManager.checkpointCounter = data.currentCheckpoint;
            KeyManager.Instance.keys = data.keys;
            ArtifactManager.artifactIds = data.artifactIds;

            PrintSaveData(data);

            CheckpointManager.Instance.UpdateCheckpoints();
            SubtitleManager.Instance.UpdateSubtitles();
        }
    }

    public static void PrintSaveData(SaveData data)
    {
        Debug.Log("----- Save Data -----");
        Debug.Log("Player position: (" + data.playerPosX + ", " + data.playerPosY + ", " + data.playerPosZ + ")");
        Debug.Log("Tool fuel: " + data.toolFuel);
        Debug.Log("Checkpoint counter: " + data.currentCheckpoint);

        string message = "Keys: ";
        foreach (string key in data.keys)
        {
            message += key + ", ";
        }
        Debug.Log(message);

        message = "Artifacts: ";
        foreach (string artifact in data.artifactIds)
        {
            message += artifact + ", ";
        }
        Debug.Log(message);

        Debug.Log("---------------------");
    }
}
