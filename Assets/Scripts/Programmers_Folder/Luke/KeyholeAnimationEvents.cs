/*
    File name: KeyholeAnimationEvents.cs
    Author:    Luke Lazzaro
    Summary: Small script that lets the keyhole's animator send animation events
    Creation Date: 21/07/2020
    Last Modified: 19/10/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyholeAnimationEvents : MonoBehaviour
{
    public void OpenDoor()
    {
        // TODO: Figure out why OpenDoor doesn't run, also tell Pat to add animation event to the correct animation clip when this is done

        Debug.Log("door animation event");

        Keyhole keyhole = transform.parent.gameObject.GetComponent<Keyhole>();
        if (keyhole != null)
        {
            if (!KeyManager.Instance.keys.Contains(keyhole.id))
            {
                Debug.Log("No key matches this keyhole.");
                return;
            }

            keyhole.Open();
            keyhole.animatedModel.SetTrigger("Lock In");
            keyhole.PlaceKeyOnKeyhole();
        }
        else
        {
            Debug.Log("no keyhole idiot");
        }
    }
}
