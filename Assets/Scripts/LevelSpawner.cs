using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSpawner : MonoBehaviour
{
    public ItemInventory basicInventory;
    public ItemInventory fanInventory;
    public ItemInventory magnetInventory;
    public ItemInventory pushInventory;
    public ItemInventory powerInventory;

    public LevelData[] levelData;
    public GameObject levelObject;
    public GameObject levelEventsObject;
    public GameObject startingPoint;
    public GameObject inventoryObject;

    public bool gameStarted = false;
    public bool levelPassed = false;
    public bool firstPress = false;

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

    void Start()
    {
        LoadLevel(0);
        
    }


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
        if(levelData[level].attempts > 5)
        {
            clippyManager.PlayHint(19);
        }



        popupInstance.ClosePopup();


        levelObject = Instantiate(levelData[level].sceneObject);

        startingPoint = Instantiate(levelData[level].transparentStartPoint, levelObject.transform);

        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
        powerInventory.SetQuantity(levelData[level].powersInventory);

        title.text = " Inventory - Level " + levelData[level].levelNumber.ToString();
        levelTitle.text = "Level " + levelData[level].levelNumber.ToString() + " - " + levelData[level].levelName;
        levelDescription.text = levelData[level].levelDescription.ToString();
        levelNumber = levelData[level].levelNumber;
        pressFatigued = false;
        presses = 0;



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

        if (levelData[level].inventoryActive == true)
        {
            inventoryObject.SetActive(true);

        }
        else
        {
            inventoryObject.SetActive(false);
            title.text = "";
        }


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
                clippyManager.PlayHint(10);
                break;
            case 6:
                clippyManager.PlayHint(11);
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
        }
    }

    public void StartPlay()
    {
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

        foreach (DelayedStay delayedStay in FindObjectsOfType<DelayedStay>())
        {
            delayedStay.SendMessage("ResetGoal", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ResetLevel()
    {
        LoadLevel(levelNumber);
    }

    public void ClearLevels()
    {
        Destroy(levelObject);
    }

    public void LevelFailed(int reason)
    {
        gameStarted = false;

        if (reason == 0)
        {
            popupMaker.Generate("Error - Ran Out Of Time!", "Level " + levelData[levelNumber].levelNumber + " Failed, Try going a bit faster?", "Error");
        }
        else if(reason == 1)
        {
            popupMaker.Generate("Error - Key fell off map", "Level " + levelData[levelNumber].levelNumber + " Failed, Try keep it all on board next time...", "Error");
        }
        else if(reason == 2)
        {
            popupMaker.Generate("Error - Hit by a Laser", "Level " + levelData[levelNumber].levelNumber + " Failed, Try avoid those pesky lasers next time?", "Error");
        }
        else if(reason == 3)
        {
            popupMaker.Generate("Error - Key flew away", "Level " + levelData[levelNumber].levelNumber + " Failed, We don't have an infinite supply! Keep them on the board...", "Error");
        }
    }

    public void LevelPassed()
    {
        levelData[levelNumber].attempts = 0;
        levelPassed = true;
        gameStarted = false;

        float completionTime = Time.time - startTime;
        if (levelData[levelNumber].personalRecord == 0 || completionTime < levelData[levelNumber].personalRecord)
        {
            levelData[levelNumber].personalRecord = completionTime;
        }

        if (levelData[levelNumber].timeLimit >= 1)
        {
            popupMaker.Generate("Victory - Level Passed", "Level " + levelData[levelNumber].levelNumber + " Passed with " + (timerFloat).ToString("0.00") + " Seconds Left", "Victory");
        }
        else
        {
            popupMaker.Generate("Victory - Level Passed", "Level " + levelData[levelNumber].levelNumber + " Passed", "Victory");
        }
    }

    public void Update()
    {
        if (timerOn)
        {
            if (gameStarted && firstPress)
            {
                timerFloat -= Time.deltaTime;

                if (timerFloat <= 0)
                {
                    LevelFailed(0);
                }

                DisplayTime();
            }
            else if (gameStarted == false)
            {
                DisplayTime();
            }
        }
        if (pressesTracked)
        {
            DisplayPresses();
        }

        if(PlayerPrefs.GetInt("SlowMode") == 1)
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
    public void DisplayTime()
    {
        float timelimit = levelData[levelNumber].timeLimit;
        timerFill.fillAmount = timerFloat / timelimit;

        int timeInSecondsInt = (int)timerFloat;
        int minutes = timeInSecondsInt / 60;
        int seconds = timeInSecondsInt - (minutes * 60);
        timerTime.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void DisplayPresses()
    {
        pressesFill.fillAmount = presses / levelData[levelNumber].pressLimits;

        if (presses > levelData[levelNumber].pressLimits)
        {
            pressesText.text = levelData[levelNumber].pressLimits + "/" + levelData[levelNumber].pressLimits;
        }
        else
        {
            pressesText.text = presses.ToString() + "/" + levelData[levelNumber].pressLimits;
        }
        if (presses >= levelData[levelNumber].pressLimits)
        {
            pressFatigued = true;
        }
    }
}