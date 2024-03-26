using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelHover : MonoBehaviour
{
    public TextMeshProUGUI levelDescription;

    public TextMeshProUGUI pressFatigueText;
    public TextMeshProUGUI personalRecord;
    public TextMeshProUGUI timeLimit;
    public LevelData levelData;
    public Sprite[] levelImages = new Sprite[10];
    public Image levelKeyboardImage;


    public void DisplayInformation(int levelInt)
    {
        levelData = GameObject.Find("GameManager").GetComponent<LevelSpawner>().levelData[levelInt];
        levelKeyboardImage.sprite = levelImages[levelInt];

        levelDescription.text = levelData.levelDescription;
        if(levelData.pressLimits >= 1)
        {
            pressFatigueText.text = levelData.pressLimits.ToString() + " Presses";
        }
        else
        {
            pressFatigueText.text = "No Limit";
        }
        if(levelData.personalRecord >= 1)
        {
            personalRecord.text = levelData.personalRecord.ToString() + " Seconds";
        }
        else
        {
            personalRecord.text = "N/A";
        }
        if (levelData.timeLimit >= 1)
        {
            timeLimit.text = levelData.timeLimit.ToString() + " Seconds";
        }
        else
        {
            timeLimit.text = "No Limit";
        }
    }

}
