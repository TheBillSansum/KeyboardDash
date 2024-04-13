using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCriteria : MonoBehaviour
{
    public bool Triggers = true; // Determines if the level completion is triggered by the player entering the trigger area
    public bool multipleStays = false; // Indicates if the level completion requires all DelayedStay objects to complete
    public bool clearCannonBalls = false; // Indicates whether all cannonballs should be disposed upon level completion
    public bool clearKeyboardChecker = false; // Indicates if the level completion criteria involves clearing a keyboard checker
    public int objectsInTrigger = 1; // The number of objects currently within the trigger area
    public Cannon cannon; // Reference to the Cannon component for disposing cannonballs
    public bool firstPress = true; // Flag to track if the first key press has occurred for keyboard check
    public DelayedStay[] delayedStay = new DelayedStay[5]; // Array of DelayedStay objects to track completion

    private LevelSpawner levelSpawner; // Reference to the LevelSpawner component

    public void Start()
    {
        // Find and initialize the LevelSpawner component
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();
        // Restart the criteria
        Restart();
    }

    public void Restart()
    {
        if (clearKeyboardChecker)
        {
            // Reset the number of objects in the trigger area
            objectsInTrigger = 1;
            // Reset the flag for the first keyboard press
            firstPress = true;
        }
    }

    public void Update()
    {
        if (multipleStays)
        {
            // Check if all delayed stays are complete
            AllDelayedStaysComplete();
        }

        if (Input.anyKeyDown && firstPress && clearKeyboardChecker)
        {
            // Decrement the count of objects in the trigger area upon the first key press
            objectsInTrigger--;
            // Set the flag to false to prevent further key presses from affecting the count
            firstPress = false;
        }

        if (clearKeyboardChecker && objectsInTrigger == 0 && levelSpawner.gameStarted == true && firstPress == false)
        {
            // Trigger level completion if all conditions are met
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
            // Increment the count of objects in the trigger area
            objectsInTrigger++;
        }

        if (Triggers)
        {
            if (other.gameObject.CompareTag("Player") && levelSpawner.gameStarted == true)
            {
                // Trigger level completion
                LevelPassed();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn") && levelSpawner.gameStarted == true)
        {
            // Decrement the count of objects in the trigger area
            objectsInTrigger--;
        }
    }

    public void SetUpObjects()
    {
        // Decrease the count of objects in the trigger area
        objectsInTrigger--;
    }

    public void LevelPassed()
    {
        if (clearCannonBalls)
        {
            // Dispose all cannonballs if required
            cannon.DisposeAllCannonballs();
        }
        // Trigger the LevelSpawner to signal level completion
        levelSpawner.LevelPassed();
    }
}
