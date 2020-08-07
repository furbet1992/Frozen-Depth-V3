using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRay : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 20;

    public AudioSource earthQuake;
    public AudioSource smallrockSlide;
    public AudioSource largeRockSlide;

    public GameObject fallingStones;
    public GameObject fallingStones2; 




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

                if (rayhit.collider.gameObject.name == "Artifact")
                {

                    //delete that artifact
                    Destroy(rayhit.collider.gameObject); 
                    //play sound of earthquake
                    earthQuake.Play();
                    smallrockSlide.Play();
                    fallingStones.SetActive(true); 
                    StartCoroutine(nextSound());
                    //floor need to open and player falls
                }
            }
        }

        IEnumerator nextSound()
        {
            yield return new WaitForSeconds(5);
            largeRockSlide.Play(); 
            fallingStones2.SetActive(true); 
            //the floor gates to opena
            
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distanceRay); 

    }
}
