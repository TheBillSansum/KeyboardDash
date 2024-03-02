using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public GameObject objectToSpawn;

    public void Start()
    {
        objectToSpawn = GameObject.FindGameObjectWithTag("Finish");
    }

    public void SpawnObject()
    {
        objectToSpawn.SetActive(true);
    }

}
