using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRay : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 20; 



    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out rayhit, distanceRay))
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (rayhit.collider.gameObject.tag == "Meltable_wall")
                {
                    Destroy(rayhit.collider.gameObject); 
                }
            }
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distanceRay); 

    }
}
