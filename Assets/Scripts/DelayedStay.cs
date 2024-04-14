using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Utilised for the goals that require the player to stay inside the goal
/// </summary>
public class DelayedStay : MonoBehaviour
{
    public float requiredTime; //Seconds required inside the goal to pass
    public float currentTime; //Current time the key has been inside the goal
    public Material gradient; //Colour of the goal, starts red and transitions to green as required time is reached
    public Image fillImage; //Image that is on top of the goal that fills with radial 360
    public TextMeshProUGUI fillPercentage; //Text displaying the fill percentage "10%"
    public bool runOnce = false; //Ensures the pass function only runs once
    public bool inTrigger = false; //If the key is currently in the goal
    public bool onlyGoal = true; //If there is only one goal, or if this goal should wait for an entire array to be full before passing
    public bool complete = false; //If current time has reached required time

    public FinishCriteria finishCriteria; //Ref to the script that deals with passing, all levels ref this script

    public delegate void ResetGoalDelegate();
    public static event ResetGoalDelegate OnResetGoal;

    private void Start()
    {
        runOnce = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) //Makes sure only the player key triggers goal
        {
            currentTime += Time.deltaTime; //Count up
            inTrigger = true; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    /// <summary>
    /// Reset to the starting state
    /// </summary>
    public void ResetGoal()
    {
        runOnce = false;
        inTrigger = false;
        complete = false;
        currentTime = 0;
    }

    public void Update()
    {
        UpdateGradient();

        if (inTrigger == false && currentTime > 0) //If not in goal and any time has been saved
        {
            currentTime -= (Time.deltaTime * 0.5f); //Start counting down at half the speed of it going up
        }
    }
    /// <summary>
    /// Runs on Update, updates the gradient, text, fill and also checks if goal has been reached
    /// </summary>
    private void UpdateGradient()
    {
        float ratio = Mathf.Clamp01(currentTime / requiredTime); //Calculate the percentage of goal complete
        fillImage.fillAmount = Mathf.Clamp01(currentTime / requiredTime); //Set the radial 360 fill to the percentage
        fillPercentage.text = (ratio * 100).ToString("0") + "%"; //Set the text to display the percentage with a '%' after 

        Color color = new Color(1f, 0f, 0f, 1f);
        color = Color.Lerp(color, new Color(0f, 1f, 0f, 0.5f), ratio);

        gradient.color = color; //Set the material

        if (ratio >= 1) //If passed
        {
            if (onlyGoal) //And if there is only one goal
            {
                finishCriteria.LevelPassed(); //Set the level as passed
            }
            complete = true; //If not only goal, set bool to passed and wait until all have been set as true
        }
        else
        {
            complete = false; //If percentage goes under 100%, return complete to false
        }
    }
}