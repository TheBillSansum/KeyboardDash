using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCriteria : MonoBehaviour
{
    public LevelSpawner levelSpawner;
    public bool Triggers = true;
    public bool multipleStays = false;
    public DelayedStay[] delayedStay = new DelayedStay[5];


    public void Start()
    {
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();
    }

    public void Update()
    {
        if (multipleStays)
        {
            AllDelayedStaysComplete();
        }
    }

    private bool AllDelayedStaysComplete()
    {
        foreach (DelayedStay stay in delayedStay)
        {
            if (!stay.complete)
            {
                return false;
            }
        }
        LevelPassed();
        return true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Triggers)
        {
            if (other.gameObject.CompareTag("Player") && levelSpawner.gameStarted == true)
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
