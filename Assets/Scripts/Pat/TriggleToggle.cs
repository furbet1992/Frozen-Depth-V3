using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggleToggle : MonoBehaviour
{
    //Drop any Gameobject in here
    public GameObject toggleObject; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            toggleObject.SetActive(true);

            StartCoroutine(disableCaption()); 
        }
    }

    IEnumerator disableCaption()
    {
        yield return new WaitForSeconds(2);
        toggleObject.SetActive(false);
    }
}
