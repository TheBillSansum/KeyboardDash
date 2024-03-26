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
    public GameObject[] levelImages;
    public GameObject layoutSketch;
    public Image levelKeyboardImage;
    public Transform spawnLocation;


    public void DisplayInformation(int levelInt)
    {
        if (levelInt < 0 || levelInt >= levelImages.Length)
        {
            Debug.LogError("Invalid level index: " + levelInt);
            return;
        }

        levelData = GameObject.Find("GameManager").GetComponent<LevelSpawner>().levelData[levelInt];

        if (layoutSketch != null)
        {
            Destroy(layoutSketch);
        }

        GameObject prefabToInstantiate = levelImages[levelInt];
        if (prefabToInstantiate == null)
        {
            Debug.LogError("Prefab is null at index: " + levelInt);
            return;
        }

        layoutSketch = Instantiate(prefabToInstantiate, spawnLocation.position, spawnLocation.rotation, spawnLocation);

        if (levelData != null)
        {
            pressFatigueText.text = levelData.pressLimits >= 1 ? levelData.pressLimits.ToString() + " Presses" : "No Limit";
            personalRecord.text = levelData.personalRecord >= 1 ? levelData.personalRecord.ToString("0.0") + " Seconds" : "No Time Submitted";
            timeLimit.text = levelData.timeLimit >= 1 ? levelData.timeLimit.ToString() + " Seconds" : "No Limit";
        }
        else
        {
            Debug.LogError("Level data is null for index: " + levelInt);
        }
    }

}
