﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRay : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 20;

    public AudioSource earthQuake;
    public AudioSource smallrockSlide;
    public AudioSource largeRockSlide;
    public AudioSource gateOpening;
    public AudioSource hydraulicSound;

    public GameObject fallingStones;
    public GameObject fallingStones2;

    public Animator activation_doorA;
    public Animator activation_doorB;
    public Animator barrier; 

    //Button
    private LayerMask interactableAction;
    public GameObject gate;
    //public Animator gate; 
    public AudioSource button;

    //UI



    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, distanceRay))
        {

            if (Input.GetKey(KeyCode.E))
            {
             
                if (rayhit.collider.gameObject.name == "Artifact")
                {

                    //delete that artifact
                    Destroy(rayhit.collider.gameObject);

                    //play sound of earthquake
                    earthQuake.Play();
                    smallrockSlide.Play();
                    fallingStones.SetActive(true);
                   // barrier.SetBool("Raise", true);

                    StartCoroutine(nextSound());
                    //floor need to open and player falls
                }

            }

            IEnumerator nextSound()
            {
                yield return new WaitForSeconds(2);
                largeRockSlide.Play();
                fallingStones2.SetActive(true);
                //the floor gates to open
                gateOpening.Play();
                hydraulicSound.Play();
                activation_doorA.SetBool("Activate_Door", true);
                activation_doorB.SetBool("Activation_Door2", true);

                // level scene
                gate.transform.position += new Vector3(0f, 2.5f, 0f);

            }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distanceRay);

        }
    }
}

