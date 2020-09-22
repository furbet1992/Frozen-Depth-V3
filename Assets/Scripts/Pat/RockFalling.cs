using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFalling : MonoBehaviour
{

    public AudioSource rocksCrashing;
    public static int fallOnce = 0;

    public GameObject rocksFalling;
    public GameObject rocksPlacement;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (fallOnce == 0)
            {
                fallOnce = 1;
                //animate rocks falling
                // sound of rocks crashing
                rocksCrashing.Play();
                StartCoroutine(RocksFalling());
            }

        }
    }
    IEnumerator RocksFalling()
    {
        Debug.Log("rocksFalling");
        yield return new WaitForSeconds(2);
        rocksFalling.transform.position = UnityEngine.Vector3.MoveTowards(rocksFalling.transform.position, rocksPlacement.transform.position, 100);

    }
}
