using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnPoint : MonoBehaviour
{

    public GameObject[] objectsToSpawn;

    private void Start()
    {
        if (objectsToSpawn.Length == 0)
        {
            objectsToSpawn = GetComponentInParent<Room>().material;
        }
        int rand = Random.Range(0, objectsToSpawn.Length);
        GameObject instance = Instantiate(objectsToSpawn[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
