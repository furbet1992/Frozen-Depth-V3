using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindToggle : MonoBehaviour
{

    public GameObject windToggle;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            windToggle.SetActive(false); 
        }
    }



}
