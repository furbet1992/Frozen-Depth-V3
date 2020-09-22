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

    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, distanceRay))
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                if (rayhit.collider.gameObject.tag == "gun")
                {
                    //Debug.Log("hit");
                    Destroy(rayhit.collider.gameObject);
                    gunInHand.SetActive(true);
                    meltTutorial.SetActive(true);
                }

                if (rayhit.collider.gameObject.tag == "Tool Component")
                {
                    //Debug.Log("hit");
                    Destroy(rayhit.collider.gameObject);
                    CreateTutorial.SetActive(true);
                }
            }

                    if (Input.GetKey(KeyCode.T))
                    {
                        meltTutorial.SetActive(false);
                        CreateTutorial.SetActive(false);
                    }
                }

     }

}

