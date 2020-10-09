using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneMan : MonoBehaviour
{
    // Start is called before the first frame update
    public int builtNumber; 


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(builtNumber); 
        }
    }
}
