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
    public GameObject inventoryObject;

    public bool gameStarted = false;
    public bool levelPassed = false;

    public TextMeshProUGUI title;

    public TextMeshProUGUI levelTitle;
    public TextMeshProUGUI levelDescription;

    public GameObject timerBox;
    public TextMeshProUGUI timerTime;
    public Image timerFill;
    public float timerFloat;
    public bool timerOn;

    public int levelNumber;

    public PopupMaker popupMaker;
    public ClippyManager clippyManager;


    void Start()
    {
        LoadLevel(0);
    }


    public void LoadLevel(int level)
    {
        gameStarted = false;
        levelPassed = false;

        levelObject = Instantiate(levelData[level].sceneObject);
        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
        powerInventory.SetQuantity(levelData[level].powersInventory);

        title.text = " Inventory - Level " + levelData[level].levelNumber.ToString();
        levelTitle.text = "Level " + levelData[level].levelNumber.ToString() + " - " + levelData[level].levelName;
        levelDescription.text = levelData[level].levelDescription.ToString();
        levelNumber = levelData[level].levelNumber;

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

        if(levelData[level].inventoryActive == true)
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
        }


    }

    public void StartPlay()
    {
        if (levelEventsObject != null)
        {
            Destroy(levelEventsObject);
        }

        levelEventsObject = Instantiate(levelData[levelNumber].keyboardEvents, levelObject.transform);
        gameStarted = true;
    }

    public void ResetLevel()
    {
        ClearLevels();
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
            popupMaker.Generate("Error - Ran Out Of Time! - Level", levelData[levelNumber].levelNumber + " Failed, Try Turning it Off and On Again?", "Error");
            ResetLevel();
        }
    }

    public void LevelPassed()
    {
        Debug.Log("Level Passed");
        levelPassed = true;

        gameStarted = false;

        popupMaker.Generate("Victory - Level Passed", levelData[levelNumber].levelNumber + " Succsess", "Victory");
        
    }

    public void Update()
    {
        if (timerOn)
        {
            if (gameStarted)
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
    }
    public void DisplayTime()
    {
        timerFill.fillAmount = timerFloat / levelData[levelNumber].timeLimit;

        int timeInSecondsInt = (int)timerFloat;
        int minutes = timeInSecondsInt / 60;
        int seconds = timeInSecondsInt - (minutes * 60);
        timerTime.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
