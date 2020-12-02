using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRay : MonoBehaviour
{
    RaycastHit rayhit;
    Ray ray;
    float distanceRay = 3;

    public GameObject playerCamera;

    //   //falling rock sounds
    public AudioSource earthQuake;
    public AudioSource smallrockSlide;
    public AudioSource largeRockSlide;
    public AudioSource gateOpening;
    public AudioSource hydraulicSound;

   
    public GameObject fallingStones;
    public GameObject fallingStones2;

    //public GameObject leftDoor;
    //public GameObject rightDoor;
    public  bool doorOpening=false;


    //Barrier that goes up when antidote #3 is collected. 
    public Animator activation_doorA;
    public Animator activation_doorB;

    public GameObject barrier;
    [SerializeField] private Vector3 targetPos = new Vector3();
    [SerializeField] private float doorRisingSpeed = 10;
    private Vector3 originalPos;

    ArtifactDisplay artifactDisplay;  


    //Button
    private LayerMask interactableAction;
    //public GameObject gate;
    //public Animator gate;
    //public AudioSource button;

    //UI


    private void Start()
    {
        originalPos = barrier.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerCamera.activeSelf && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rayhit, distanceRay))
        {

            if (Input.GetKey(KeyCode.E))
            {
                if (rayhit.collider.gameObject.name == "Antidote (3)")
                {
                    Debug.Log("got it"); 
                    //delete that artifact
                    Destroy(rayhit.collider.gameObject);

                    //play sound of earthquake
                    earthQuake.Play();
                    smallrockSlide.Play();
                    fallingStones.SetActive(true);
                    barrier.transform.position = Vector3.MoveTowards(barrier.transform.position, originalPos + targetPos, doorRisingSpeed * Time.deltaTime);
                    doorOpening = true; 
                    StartCoroutine(nextSound());
                    //floor need to open and player falls
                }

            }

        }


            IEnumerator nextSound()
            {
                yield return new WaitForSeconds(0);
                largeRockSlide.Play();
                fallingStones2.SetActive(true);
                //the floor gates to open
                gateOpening.Play();
                hydraulicSound.Play();

                activation_doorA.SetBool("Activate_Door", true);
                activation_doorB.SetBool("Activation_Door2", true);

            // level scene
            //gate.transform.position += new Vector3(0f, 2.5f, 0f);

        }

        }
    }


