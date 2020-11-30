using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BlackScreen : MonoBehaviour
{
    public GameObject blackScreen;  



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //turn on UI; 
            blackScreen.SetActive(true); 
        }
    }
}
