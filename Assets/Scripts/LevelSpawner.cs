using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// GameManager script, used to spawn all of the levels in when required. Also runs all needed checks throughout gameplay
/// </summary>
public class LevelSpawner : MonoBehaviour
{
    #region Public Variables

    //Holds each items Inventory
    public ItemInventory basicInventory;
    public ItemInventory fanInventory;
    public ItemInventory magnetInventory;
    public ItemInventory pushInventory;
    public ItemInventory powerInventory;


    public LevelData[] levelData; //Level scriptable object
    public GameObject levelObject; //Level keyboard object
    public GameObject levelEventsObject; //Level keys
    public GameObject startingPoint; //Transparent outline of keys that despawns on play
    public GameObject inventoryObject; //The object to disable to hide the inventory

    public bool gameStarted = false;
    public bool levelPassed = false;
    public bool firstPress = false;

    public bool finalLevel = false;

    public TextMeshProUGUI title;
    public GameObject timeScaledDownText;

    public TextMeshProUGUI levelTitle;
    public TextMeshProUGUI levelDescription;

    public GameObject timerBox;
    public TextMeshProUGUI timerTime;
    public Image timerFill;
    public float timerFloat;
    public bool timerOn;
    private float startTime;

    public GameObject pressFatigueBox;
    public TextMeshProUGUI pressesText;
    public Image pressesFill;
    public bool pressesTracked;
    public float presses;
    public bool pressFatigued = false;

    public int levelNumber;
    public TransitionManager transitionManager;

    public PopupMaker popupMaker;
    public PopupInstance popupInstance;
    public ClippyManager clippyManager;

    public int levelToLoad;

    #endregion

    #region Initialization

    void Start()
    {       
        Screen.SetResolution(1920, 1080, true);
        LoadLevel(0);
    }

    public void Awake()
    {

    }
    #endregion

    #region Level Loading

    public void LoadLevel(int level)
    {
        levelToLoad = level;
        transitionManager.StartTransition();
        StartCoroutine(LoadLevelCoroutine());
    }

    private IEnumerator LoadLevelCoroutine()
    {
        float transitionDuration = 1f;
        float halfDuration = transitionDuration * 0.5f;
        yield return new WaitForSeconds(halfDuration);

        LoadingLevel();
    }

    public void LoadingLevel()
    {
        int level = levelToLoad;
        ClearLevels();
        gameStarted = false;
        levelPassed = false;
        firstPress = false;
        levelData[level].attempts++;

        if (levelData[level].attempts > 5)
        {
            clippyManager.PlayHint(25);
        }

        popupInstance.ClosePopup();

        levelObject = Instantiate(levelData[level].sceneObject);

        startingPoint = Instantiate(levelData[level].transparentStartPoint, levelObject.transform);

        // Set inventory quantities
        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
        powerInventory.SetQuantity(levelData[level].powersInventory);

        // Set UI texts
        title.text = " Inventory - Level " + levelData[level].levelNumber.ToString();
        levelTitle.text = "Level " + levelData[level].levelNumber.ToString() + " - " + levelData[level].levelName;
        levelDescription.text = levelData[level].levelDescription.ToString();
        levelNumber = levelData[level].levelNumber;
        pressFatigued = false;
        presses = 0;

        // Set timer and press limits
        if (levelData[level].timeLimit >= 1)
        {
            timerFloat = levelData[level].timeLimit;
            timerOn = true;
            timerBox.SetActive(true);
        }
        else
        {
            timerOn = false;
            timerBox.SetActive(false);
        }

        if (levelData[level].pressLimits >= 1)
        {
            pressFatigueBox.SetActive(true);
            pressesTracked = true;
        }
        else
        {
            pressFatigueBox.SetActive(false);
            pressesTracked = false;
        }

        // Activate or deactivate inventory UI
        if (levelData[level].inventoryActive == true)
        {
            inventoryObject.SetActive(true);
        }
        else
        {
            inventoryObject.SetActive(false);
            title.text = "";
        }

        // Play hint based on level number
        switch (levelNumber)
        {
            case 1:
                clippyManager.PlayHint(4);
                break;
            case 2:
                clippyManager.PlayHint(5);
                break;
            case 3:
                clippyManager.PlayHint(7);
                break;
            case 4:
                clippyManager.PlayHint(8);
                break;
            case 5:
                clippyManager.PlayHint(9);
                break;
            case 6:
                clippyManager.PlayHint(10);
                break;
            case 7:
                clippyManager.PlayHint(12);
                break;
            case 8:
                clippyManager.PlayHint(13);
                break;
            case 9:
                clippyManager.PlayHint(14);
                break;
            case 10:
                clippyManager.PlayHint(15);
                break;
            case 11:
                clippyManager.PlayHint(16);
                break;
            case 12:
                clippyManager.PlayHint(17);
                break;
            case 13:
                clippyManager.PlayHint(18);
                break;
            case 14:
                clippyManager.PlayHint(19);
                break;
            case 15:
                clippyManager.PlayHint(20);
                break;

        }
    }

    #endregion

    #region Game Control

    public void StartPlay()
    {
        // Destroy previous level events object
        if (levelEventsObject != null)
        {
            Destroy(levelEventsObject);
        }
        gameStarted = false;
        levelPassed = false;
        firstPress = false;
        pressFatigued = false;
        presses = 0;
        Destroy(startingPoint);
        startTime = Time.time;
        levelEventsObject = Instantiate(levelData[levelNumber].keyboardEvents, levelObject.transform);
        gameStarted = true;

        // Restart finish criteria
        GameObject obj = GameObject.Find("ClearKeyboardChecker");
        if (obj != null)
        {
            obj.GetComponent<FinishCriteria>().Restart();
        }

        // Reset goals for DelayedStay objects
        foreach (DelayedStay delayedStay in FindObjectsOfType<DelayedStay>())
        {
            delayedStay.SendMessage("ResetGoal", SendMessageOptions.DontRequireReceiver);
        }
    }
    /// <summary>
    /// Resets the current levelNumber
    /// </summary>
    public void ResetLevel()
    {
        LoadLevel(levelNumber);
    }

    /// <summary>
    /// Clears the levelObject gameobject
    /// </summary>
    public void ClearLevels()
    {
        Destroy(levelObject);
    }

    /// <summary>
    /// Generates a popup for different failure cases
    /// <para>0 = Time Limit</para>
    /// <para>1 = Key fell off map</para>
    /// <para>2 = Key hit by laser</para>
    /// <para>3 = Key flew away (zero gravity)</para>
    /// </summary>
    /// <param name="reason"></param>
    public void LevelFailed(int reason)
    {
        gameStarted = false;
        if (reason == 0)
        {
            popupMaker.Generate("Error - Ran Out Of Time!", "Level " + levelData[levelNumber].levelNumber + " Failed, Try going a bit faster?", "Error");
        }
        else if (reason == 1)
        {
            popupMaker.Generate("Error - Key fell off map", "Level " + levelData[levelNumber].levelNumber + " Failed, Try keep it all on board next time...", "Error");
        }
        else if (reason == 2)
        {
            popupMaker.Generate("Error - Hit by a Laser", "Level " + levelData[levelNumber].levelNumber + " Failed, Try avoid those pesky lasers next time?", "Error");
        }
        else if (reason == 3)
        {
            popupMaker.Generate("Error - Key flew away", "Level " + levelData[levelNumber].levelNumber + " Failed, We don't have an infinite supply! Keep them on the board...", "Error");
        }
    }

    /// <summary>
    /// Functionality to pass the level and move onto the next, displays a pop up with a custom message
    /// </summary>
    public void LevelPassed()
    {
        levelData[levelNumber].attempts = 0; //Reset attempts to 0
        levelPassed = true;
        gameStarted = false;

        float completionTime = Time.time - startTime; //Calculate how much time the level took to pass

        if (levelData[levelNumber].personalRecord == 0 || completionTime < levelData[levelNumber].personalRecord) //If a new personal record for that level
        {
            levelData[levelNumber].personalRecord = completionTime; //Save it to that levels scriptable object
        }

        if (levelNumber != 15)
        {

            if (levelData[levelNumber].timeLimit >= 1) //If that level had a time limit, tell the player how long they had left
            {
                popupMaker.Generate("Victory - Level Passed", "Level " + levelData[levelNumber].levelNumber + " Passed with " + (timerFloat).ToString("0.00") + " Seconds Left", "Victory");
            }
            else //If no time limit, just say a normal victory message
            {
                popupMaker.Generate("Victory - Level Passed", "Level " + levelData[levelNumber].levelNumber + " Passed", "Victory");
            }
        }
        else //If the last level, display a custom message with custom buttons
        {
            popupMaker.Generate("Game Over - You Won!", "Congratulations, you completed the game, more levels are coming! Thank You for playing :)", "Game Win");
        }
    }

    #endregion

    #region Update

    public void Update()
    {
        // Update timer and presses display
        if (timerOn)
        {
            if (gameStarted && firstPress) //Once the first press has been tracked start counting
            {
                timerFloat -= Time.deltaTime; //Count down 

                if (timerFloat <= 0) //If time has run out 
                {
                    LevelFailed(0); //End the run and display a custom fail message
                }

                DisplayTime();
            }
            else if (gameStarted == false) //Display the time but don't start counting down yet
            {
                DisplayTime();
            }
        }
        if (pressesTracked) //If press fatigue is on display the ui
        {
            DisplayPresses();
        }

        // Handle slow mode
        if (PlayerPrefs.GetInt("SlowMode") == 1)
        {
            Time.timeScale = 0.75f;
            timeScaledDownText.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            timeScaledDownText.SetActive(false);
        }
    }

    #endregion

    #region UI Display

    /// <summary>
    /// If time limit is active for this level, display the UI window for it
    /// </summary>
    public void DisplayTime()
    {
        float timelimit = levelData[levelNumber].timeLimit;
        timerFill.fillAmount = timerFloat / timelimit; //Calculate the fill on the circle 

        int timeInSecondsInt = (int)timerFloat; 
        int minutes = timeInSecondsInt / 60;
        int seconds = timeInSecondsInt - (minutes * 60);
        timerTime.text = minutes.ToString("D2") + ":" + seconds.ToString("D2"); //Display the time in text, Example = "0:29"
    }

    /// <summary>
    /// If press fatigue is active for this level display the UI window for it
    /// </summary>
    public void DisplayPresses()
    {
        pressesFill.fillAmount = presses / levelData[levelNumber].pressLimits; //Calculate the fill on the circle 

        if (presses > levelData[levelNumber].pressLimits)
        {
            pressesText.text = levelData[levelNumber].pressLimits + "/" + levelData[levelNumber].pressLimits; //Display the progress in text, Example = "3/6"
        }
        else
        {
            pressesText.text = presses.ToString() + "/" + levelData[levelNumber].pressLimits;
        }
        if (presses >= levelData[levelNumber].pressLimits) //If too many presses, stop all further presses
        {
            pressFatigued = true;
        }
    }

    #endregion
}
