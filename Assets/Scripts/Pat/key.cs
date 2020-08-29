using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

   static bool keyCollected; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            keyCollected = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false; 
        }
    }
}
