using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


/// <summary>
/// Script to spawn pop ups such as failures and level passes
/// </summary>
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


    /// <summary>
    /// Function to generate a pop up with custom text, buttons and images
    /// </summary>
    /// <param name="Title">Top text of the pop up</param>
    /// <param name="Body">Main body of text for the popup</param>
    /// <param name="Image"> Which of the images to use, options: warningSprite, errorSprite, victorySprite or gameComplete</param>
    public void Generate(string Title, string Body, string Image)
    {
        
        lostButtons.SetActive(false); //Ensure all previous buttons have been removed
        victoryButtons.SetActive(false);
        gameFinished.SetActive(false);

        popUpObjectBlank.SetActive(true); //Set the pop up object to active
        popUpTitle.text = Title; //Set the title text
        popUpBody.text = Body; //And the description

        switch (Image)
        {
            case "Warning":
                popUpImage.sprite = warningSprite; //Set the specfic sprite
                break;

            case "Error":
                popUpImage.sprite = errorSprite;
                lostButtons.SetActive(true); //If custom buttons are utilised, turn them on
                break;

            case "Victory":
                popUpImage.sprite = victorySprite;
                victoryButtons.SetActive(true);
                break;

            case "Game Win":
                popUpImage.sprite = gameComplete;
                gameFinished.SetActive(true);
                break;
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
    /// <summary>
    /// Load the next level in the array of levels, if there is another.
    /// </summary>
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

    /// <summary>
    /// Reload the current level
    /// </summary>
    public void ResetButton()
    {
        levelSpawner.ResetLevel();
    }
}
