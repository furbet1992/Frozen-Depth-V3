using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class startLevel : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 20;
    public GameObject interactable;

    public GameObject meltTutorial;
    public GameObject CreateTutorial;

    public GameObject gunInHand;


    public AudioSource rocksCrashing;
    public GameObject rocksFalling;
    public GameObject rocksPlacement;

    public static int fallOnce = 0;


    // public GameObject gunProp; 


    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, distanceRay))
        {
            

                if (rayhit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    interactable.SetActive(true);
                }
                if (rayhit.collider.gameObject.layer != LayerMask.NameToLayer("Interactable"))
                {
                    interactable.SetActive(false);
                }



            if (Input.GetKey(KeyCode.E))
            {
                if (rayhit.collider.gameObject.tag == "gun")
                {
                    //Debug.Log("hit");
                    Destroy(rayhit.collider.gameObject);
                    gunInHand.SetActive(true);
                    meltTutorial.SetActive(true);
                }

                if (rayhit.collider.gameObject.tag == "gun2")
                {
                    //Debug.Log("hit");
                    Destroy(rayhit.collider.gameObject);
                    CreateTutorial.SetActive(true);
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

                    if (Input.GetKey(KeyCode.T))
                    {
                        meltTutorial.SetActive(false);
                        CreateTutorial.SetActive(false);
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

