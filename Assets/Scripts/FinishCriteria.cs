using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCriteria : MonoBehaviour
{
    public LevelSpawner levelSpawner;



    public void Start()
    {
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            levelSpawner.LevelPassed();
        }
    }
}
