/*
    File name: ManagerOfTheTerrainManagers.cs
    Author: Alex Mollard
    Summary: Used to load all managers at the start of a level.
    Creation Date: 12/10/2020
    Last Modified: 12/10/2020
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ManagerOfTheTerrainManagers : MonoBehaviour
{
    [Header("Managers")]
    [Tooltip("All terrain managers")]
    public TerrainMan[] terrainMans = null;

    [Header("Ui Variables")]
    [Tooltip("Visual input on how many managers are loaded and or being cached")]
    public GameObject loadingScreen = null;
    public Slider loadingBar = null;
    public TextMeshProUGUI cachingText = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoLoad());
    }

    IEnumerator DoLoad()
    {
        for (int i = 0; i < terrainMans.Length; i++)
        {
            if (terrainMans[i].StartCreation())
            {
                cachingText.text = "Caching current terrain (" + i + "/" + terrainMans.Length + ")...";
            }
            else
            {
                cachingText.text = "Reading From Cache (" + i + "/" + terrainMans.Length + ")...";
            }

            loadingBar.value = (float)(i + 1) * (1.0f / (float)terrainMans.Length);
            yield return 0;
        }

        loadingScreen.gameObject.SetActive(false);
    }
}
