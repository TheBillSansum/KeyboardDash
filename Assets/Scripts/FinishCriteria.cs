using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCriteria : MonoBehaviour
{
    public LevelSpawner levelSpawner;
    public bool Triggers = true;
    public bool multipleStays = false;
    public bool clearCannonBalls = false;
    public bool clearKeyboardChecker = false;
    public int objectsInTrigger = 1;
    public Cannon cannon;
    public bool firstPress = true;
    public DelayedStay[] delayedStay = new DelayedStay[5];


    public void Start()
    {
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();
        Restart();
    }

    public void Restart()
    {
        if (clearKeyboardChecker)
        {
            objectsInTrigger = 1;

            firstPress = true;
        }
    }



    public void Update()
    {
        if (multipleStays)
        {
            AllDelayedStaysComplete();
        }       

        if (Input.anyKeyDown && firstPress && clearKeyboardChecker)
        {
            objectsInTrigger--;
            firstPress = false;
        }

        if (clearKeyboardChecker && objectsInTrigger == 0 && levelSpawner.gameStarted == true && firstPress == false)
        {
            LevelPassed();
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
        if (clearKeyboardChecker && other.CompareTag("Respawn") && levelSpawner.gameStarted == true)
        {
            objectsInTrigger++;
        }

        if (Triggers)
        {
            if (other.gameObject.CompareTag("Player") && levelSpawner.gameStarted == true)
            {
                LevelPassed();
            }
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn") && levelSpawner.gameStarted == true)
        {
            objectsInTrigger--;
        }
    }
    public void SetUpObjects()
    {
        objectsInTrigger--;
    }


    public void LevelPassed()
    {
        if (clearCannonBalls)
        {
            cannon.DisposeAllCannonballs();
        }
        levelSpawner.LevelPassed();
    }
}
