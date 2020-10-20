/*
    File name: KeyholeAnimationEvents.cs
    Author:    Luke Lazzaro
    Summary: Small script that lets the keyhole's animator send animation events
    Creation Date: 21/07/2020
    Last Modified: 20/10/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyholeAnimationEvents : MonoBehaviour
{
    public void OpenDoor()
    {
        Keyhole keyhole = transform.parent.gameObject.GetComponent<Keyhole>();
        if (keyhole != null)
        {
            if (!KeyManager.Instance.keys.Contains(keyhole.id))
            {
                Debug.LogError("No key matches this keyhole.");
                return;
            }

            keyhole.Open();
        }
        else
        {
            Debug.LogError("Keyhole.cs not found on parent object.");
        }
    }
}
