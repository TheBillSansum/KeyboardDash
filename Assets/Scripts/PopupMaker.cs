using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopupMaker : MonoBehaviour
{
    public GameObject popUpObjectBlank;
    public LevelSpawner levelSpawner;
    public TMP_Text popUpTitle;
    public TMP_Text popUpBody;
    public Image popUpImage;
    public GameObject victoryButtons;
    public GameObject lostButtons;
    public GameObject gameFinished;
    public PopupInstance instance;

    public Sprite warningSprite;
    public Sprite errorSprite;
    public Sprite victorySprite;
    public Sprite gameComplete;

    public void Generate(string Title, string Body, string Image)
    {
        
        lostButtons.SetActive(false);
        victoryButtons.SetActive(false);
        popUpObjectBlank.SetActive(true);
        popUpTitle.text = Title;
        popUpBody.text = Body;

        if (Image == "Warning")
        {
            popUpImage.sprite = warningSprite;

        }
        else if (Image == "Error")
        {
            popUpImage.sprite = errorSprite;
            lostButtons.SetActive(true);
            victoryButtons.SetActive(false);
            gameFinished.SetActive(false);
        }
        else if (Image == "Victory")
        {
            popUpImage.sprite = victorySprite;
            victoryButtons.SetActive(true);
            lostButtons.SetActive(false);
            gameFinished.SetActive(false);

        }
        else if(Image == "Game Win")
        {
            popUpImage.sprite = gameComplete;
            victoryButtons.SetActive(false);
            lostButtons.SetActive(false);
            gameFinished.SetActive(true);

        }
        
    }

    public void ButtonPressed()
    {
        if (levelSpawner.levelPassed == true)
        {
            if (levelSpawner.levelData[levelSpawner.levelNumber + 1] != null)
            {
                levelSpawner.LoadLevel(levelSpawner.levelNumber + 1);
            }
            else
            {
                Debug.Log("No next level");
            }
        }
    }

    public void NextLevelButton()
    {
        if (levelSpawner.levelData[levelSpawner.levelNumber + 1] != null)
        {
            levelSpawner.LoadLevel(levelSpawner.levelNumber + 1);
        }
        else
        {
            Debug.Log("No next level");
        }
    }

    public void ResetButton()
    {
        levelSpawner.ResetLevel();
    }
}
