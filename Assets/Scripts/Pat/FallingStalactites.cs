using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStalactites : MonoBehaviour
{
    //the  object falling to be istantitated. 
    public GameObject fallingObject;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay; 




    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay); 
    }

    public void SpawnObject()
    {
        GameObject cloney= Instantiate(fallingObject, transform.position, Quaternion.identity) as GameObject;
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject"); 
        }
        Destroy(cloney, 2.0f); 
    }
}
