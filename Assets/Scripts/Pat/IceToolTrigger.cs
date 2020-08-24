using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class IceToolTrigger : MonoBehaviour
{
    public AudioSource rocksCrashing;
    public GameObject rocksFalling;
    public GameObject staticRocks;

    public static int fallOnce = 0; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && fallOnce == 0)
        {
            fallOnce = 1; 
            //animate rocks falling
            // sound of rocks crashing
            rocksCrashing.Play();
            StartCoroutine(RocksFalling());
 
          
        }
    }

    IEnumerator RocksFalling()
    {
        yield return new WaitForSeconds(2);
        rocksFalling.transform.position = UnityEngine.Vector3.MoveTowards(rocksFalling.transform.position, staticRocks.transform.position, 100); 

    }

}
