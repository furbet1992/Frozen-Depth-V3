using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggleToggle : MonoBehaviour
{
    //Drop any Gameobject in here
    public GameObject toggleObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            toggleObject.SetActive(true);
            StartCoroutine(turnOffUi());
        }
    }

    IEnumerator turnOffUi()
    {
        yield return new WaitForSeconds(1);
        toggleObject.SetActive(false); 
    }
}
