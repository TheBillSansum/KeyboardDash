using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilised in every level to move to the next level when it has been completed
/// </summary>
public class FinishCriteria : MonoBehaviour
{
    public bool Triggers = true; // Determines if the level completion is triggered by the player entering the trigger area
    public bool multipleStays = false; // Indicates if the level completion requires all DelayedStay objects to complete
    public bool clearCannonBalls = false; // Indicates whether all cannonballs should be disposed upon level completion
    public bool clearKeyboardChecker = false; // If the game needs to check if objects are clear from the keyboard, utilised for one level
    public int objectsInTrigger = 1; // The number of objects currently within the trigger area
    public Cannon cannon; // Reference to the Cannon component for disposing cannonballs
    public bool firstPress = true; // Bool to track if the first key press has occurred for keyboard check
    public DelayedStay[] delayedStay = new DelayedStay[5]; // Array of DelayedStay objects to track completion

    private LevelSpawner levelSpawner; // Reference to the LevelSpawner component

    public void Start()
    {
       
        levelSpawner = GameObject.Find("GameManager").GetComponent<LevelSpawner>();  // Find and initialize the LevelSpawner component   
        Restart(); // Restart the criteria
    }

    public void Restart()
    {
        if (clearKeyboardChecker)
        {         
            objectsInTrigger = 1; // Reset the number of objects in the trigger area   
            firstPress = true; // Reset the bool for the first keyboard press
        }
    }

    public void Update()
    {
        if (multipleStays)
        {    
            AllDelayedStaysComplete(); // Check if all delayed stays are complete
        }

        if (Input.anyKeyDown && firstPress && clearKeyboardChecker) //This is done to fix a bug that ended the game straight away before the triggers could react
        {
            objectsInTrigger--; // Decrement the count of objects in the trigger area upon the first key press
           
            firstPress = false; // Set the bool to false to prevent further key presses from affecting the count
        }

        if (clearKeyboardChecker && objectsInTrigger == 0 && levelSpawner.gameStarted == true && firstPress == false)
        {           
            LevelPassed(); // Trigger level completion if all conditions are met
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
            objectsInTrigger++; // Increment the count of objects in the trigger area
        }

        if (Triggers)
        {
            if (other.gameObject.CompareTag("Player") && levelSpawner.gameStarted == true)
            {             
                LevelPassed(); // Trigger level completion
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn") && levelSpawner.gameStarted == true)
        {          
            objectsInTrigger--; // Decrement the count of objects in the trigger area
        }
    }

    public void SetUpObjects()
    {     
        objectsInTrigger--; // Decrease the count of objects in the trigger area
    }

    public void LevelPassed()
    {
        if (clearCannonBalls)
        {          
            cannon.DisposeAllCannonballs(); // Dispose all cannonballs if required
        }     
        levelSpawner.LevelPassed(); // Trigger the LevelSpawner to signal level completion
    }
}
