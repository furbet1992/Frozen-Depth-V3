using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnergySource : MonoBehaviour
{
    static bool sourceCollected;
    public GameObject door; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sourceCollected = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(Opendoor()); 
        }
    }

    IEnumerator Opendoor()
    {
        yield return new WaitForSeconds(1);
        door.transform.position += new UnityEngine.Vector3(0f, 4.5f, 0f);
    }
}