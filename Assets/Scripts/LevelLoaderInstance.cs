using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Each level has their own Instance of this script to display all of the information.
/// </summary>
public class LevelLoaderInstance : MonoBehaviour
{
    public int level; //That levels int
    public LevelSpawner levelSpawner;
    public LevelData levelData;

    public TextMeshProUGUI levelText; //Title 
    public GameObject basic; //The object of the basic key sprite, to be enabled if that key is in that levels inventory atleast once
    public GameObject magnet; //And so on...
    public GameObject convey;
    public GameObject fan;
    public GameObject power;
    public GameObject timeLimit; //Sprite of the time limits object
    public GameObject pressFatigue; //And so on...
    public GameObject difficulty;
    public GameObject externalThreat;

    public Sprite laserThreat; //Sprite to swap to depending on the External Threat
    public Sprite lasersThreat;
    public Sprite cannonThreat;
    public Sprite zeroGravity;

    //All this text is normally hidden from the player, they must hover over specific sprites to see them, the functionality for hovering is handled in events

    public TextMeshProUGUI basicText; //The text for quantity of basic keys
    public TextMeshProUGUI magnetText; //And so on...
    public TextMeshProUGUI conveyText;
    public TextMeshProUGUI fanText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI fatigueText;
    public TextMeshProUGUI timeText;
    public Image difficultyColour; //Colour to change between Green, Orange and Red for difficulty level
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI cannonText; //External Hazard text 

    private Dictionary<GameObject, TextMeshProUGUI> inventoryTextMap;

    void Start()
    {
        // Retrieve level data for the current level
        levelData = levelSpawner.levelData[level];
        // Update UI to display current level information
        levelText.text = $"Level {level} - {levelData.levelName}";

        // Initialize the mapping between inventory objects and their corresponding text UI elements
        InitializeInventoryMap();

        // Update UI for each inventory item with its respective count
        UpdateInventoryUI(basic, basicText, levelData.basicInventory);
        UpdateInventoryUI(magnet, magnetText, levelData.magnetsInventory);
        UpdateInventoryUI(convey, conveyText, levelData.conveyorsInventory);
        UpdateInventoryUI(fan, fanText, levelData.fansInventory);
        UpdateInventoryUI(power, powerText, levelData.powersInventory);
        UpdateInventoryUI(pressFatigue, fatigueText, levelData.pressLimits, "Fatigue: ");
        UpdateInventoryUI(timeLimit, timeText, levelData.timeLimit, "Time: ");
    }

    /// <summary>
    /// Initializes the mapping between inventory objects and their corresponding text UI elements.
    /// </summary>
    private void InitializeInventoryMap()
    {
        // Create a dictionary to map inventory objects to their text UI elements
        inventoryTextMap = new Dictionary<GameObject, TextMeshProUGUI>
    {
        { basic, basicText },
        { magnet, magnetText },
        { convey, conveyText },
        { fan, fanText },
        { power, powerText },
        { pressFatigue, fatigueText },
        { timeLimit, timeText }
    };

        // Set UI elements based on level difficulty and description
        if (levelData.difficulty == LevelData.difficultyLevel.easy)
        {
            difficultyColour.color = Color.green;
            difficultyText.text = "-Easy-";
        }
        else if (levelData.difficulty == LevelData.difficultyLevel.medium)
        {
            difficultyColour.color = Color.yellow;
            difficultyText.text = "-Medium-";
        }
        else if (levelData.difficulty == LevelData.difficultyLevel.hard)
        {
            difficultyColour.color = Color.red;
            difficultyText.text = "-Hard-";
        }

        // Display level description and any external threats
        descriptionText.text = levelData.levelDescription.ToString();
        if (levelData.externalThreat == "Cannon")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = cannonThreat;
            cannonText.text = "-Cannon-";
        }
        else if (levelData.externalThreat == "Laser")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = laserThreat;
            cannonText.text = "-Laser-";
        }
        else if (levelData.externalThreat == "Lasers")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = lasersThreat;
            cannonText.text = "-Lasers-";
        }
        else if (levelData.externalThreat == "ZeroGravity")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = zeroGravity;
            cannonText.text = "-Zero Gravity-";
        }
        else
        {
            externalThreat.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the UI for an inventory item based on its count.
    /// </summary>
    /// <param name="inventoryObject">The GameObject representing the inventory item.</param>
    /// <param name="inventoryText">The TextMeshProUGUI component to display the inventory count.</param>
    /// <param name="inventoryCount">The count of the inventory item.</param>
    /// <param name="differenceText">Additional text to display before the count (optional).</param>
    private void UpdateInventoryUI(GameObject inventoryObject, TextMeshProUGUI inventoryText, float inventoryCount, string differenceText = "")
    {
        if (inventoryCount > 0) // Check if the inventory count is greater than 0 to determine visibility
        {
            inventoryObject.SetActive(true);
            inventoryText.text = $"-{differenceText}{inventoryCount.ToString("0")}-"; // Update text to display inventory count with optional additional text
        }
        else
        {
            inventoryObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the state of a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject whose state to update.</param>
    /// <param name="state">The new state of the GameObject.</param>
    private void UpdateGameObjectState(GameObject obj, bool state)
    {
        obj.SetActive(state); // Set the GameObject's active state
    }

    /// <summary>
    /// Initiates the loading of the current level.
    /// </summary>
    public void PlayLevel()
    {
        levelSpawner.LoadLevel(level); // Load the current level using the level spawner
    }
}
