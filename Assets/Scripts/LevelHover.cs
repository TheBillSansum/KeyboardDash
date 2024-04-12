using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Master script for displaying information from the 'Level Loader' menu. 
/// <para> Displays content such as the layout sketch, title and personal record, ect...</para>
/// </summary>
public class LevelHover : MonoBehaviour
{
    public TextMeshProUGUI levelDescription;

    public TextMeshProUGUI pressFatigueText;
    public TextMeshProUGUI personalRecord;
    public TextMeshProUGUI timeLimit;
    public TextMeshProUGUI title;
    public LevelData levelData;
    public GameObject[] levelImages;
    public GameObject layoutSketch;
    public Image levelKeyboardImage;
    public Transform spawnLocation;

    /// <summary>
    /// Takes in the the int of the level and displays that information
    /// <para> All information is saved in the GameManager which this references</para>
    /// </summary>
    /// <param name="levelInt"></param>
    public void DisplayInformation(int levelInt)
    {
        if (levelInt < 0 || levelInt >= levelImages.Length)
        {
            Debug.LogError("Invalid level index: " + levelInt);
            return;
        }

        levelData = GameObject.Find("GameManager").GetComponent<LevelSpawner>().levelData[levelInt]; //Gets the GameManager

        if (layoutSketch != null) //Destroys the old layout sketch
        {
            Destroy(layoutSketch);
        }

        GameObject prefabToInstantiate = levelImages[levelInt];
        if (prefabToInstantiate == null)
        {
            Debug.LogError("Prefab is null at index: " + levelInt);
            return;
        }

        layoutSketch = Instantiate(prefabToInstantiate, spawnLocation.position, spawnLocation.rotation, spawnLocation); //Spawns the new layout sketch and saves the reference for later deletion

        if (levelData != null) //Ensure no errors
        {
            title.text = "Level " + levelData.levelNumber + " Layout - Windows Paint";
            pressFatigueText.text = levelData.pressLimits == 0 ? "No Limit" : levelData.pressLimits == 1 ? levelData.pressLimits.ToString() + " Press" : levelData.pressLimits.ToString() + " Presses";
            personalRecord.text = levelData.personalRecord >= 1 ? levelData.personalRecord.ToString("0.0") + " Seconds" : "No Time Submitted";
            timeLimit.text = levelData.timeLimit >= 1 ? levelData.timeLimit.ToString() + " Seconds" : "No Limit";
        }
        else
        {
            Debug.LogError("Level data is null for index: " + levelInt);
        }
    }

}
