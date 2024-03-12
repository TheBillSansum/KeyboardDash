using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCriteria : MonoBehaviour
{
    public LevelSpawner levelSpawner;
    public bool Triggers = true;


    public void Start()
    {
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Triggers)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                LevelPassed();
            }
        }
    }


    public void LevelPassed()
    {
        levelSpawner.LevelPassed();
    }
}
