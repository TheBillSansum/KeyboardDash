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

    public bool gameStarted = false;

    public TextMeshProUGUI title;

    public GameObject timerBox;
    public TextMeshProUGUI timerTime;
    public Image timerFill;
    public float timerFloat;
    public bool timerOn;

    public int levelNumber;

    public PopupMaker popupMaker;


    void Start()
    {
        LoadLevel(0);
    }


    public void LoadLevel(int level)
    {
        gameStarted = false;

        levelObject = Instantiate(levelData[level].sceneObject);
        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
        powerInventory.SetQuantity(levelData[level].powersInventory);

        title.text = "Level " + levelData[level].levelNumber.ToString() + " - " + levelData[level].levelName;
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
        levelObject.gameObject.SetActive(false);
        LoadLevel(0);
    }

    public void LevelFailed(int reason)
    {
        gameStarted = false;

        if (reason == 0)
        {
            popupMaker.Generate("Error - Ran Out Of Time!", levelData[levelNumber].levelName + " Failed, Try Turning it Off and On Again?", "Error");
        }
    }

    public void LevelPassed()
    {
        Debug.Log("Level Passed");

        gameStarted = false;

        popupMaker.Generate("Victory - Level Passed", levelData[levelNumber].levelName + " Succsess", "Error");
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
