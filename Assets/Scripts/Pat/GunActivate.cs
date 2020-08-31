using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActivate : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 20;

    //gun activation
    public GameObject gunActivation;




    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, distanceRay))
        {
            if (rayhit.collider.gameObject.tag == "gun")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gunActivation.SetActive(true);
                    Debug.Log("gunActivated");
                }
            }

        }
    }
}
