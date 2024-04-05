using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelLoaderInstance : MonoBehaviour
{
    public int level;
    public LevelSpawner levelSpawner;
    public LevelData levelData;

    public TextMeshProUGUI levelText;
    public GameObject basic;
    public GameObject magnet;
    public GameObject convey;
    public GameObject fan;
    public GameObject power;
    public GameObject timeLimit;
    public GameObject pressFatigue;
    public GameObject difficulty;
    public GameObject externalThreat;

    public Sprite laserThreat;
    public Sprite lasersThreat;
    public Sprite cannonThreat;
    public Sprite zeroGravity;


    public TextMeshProUGUI basicText;
    public TextMeshProUGUI magnetText;
    public TextMeshProUGUI conveyText;
    public TextMeshProUGUI fanText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI fatigueText;
    public TextMeshProUGUI timeText;
    public Image difficultyColour;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI cannonText;

    private Dictionary<GameObject, TextMeshProUGUI> inventoryTextMap;

    void Start()
    {
        levelData = levelSpawner.levelData[level];
        levelText.text = $"Level {level} - {levelData.levelName}";

        InitializeInventoryMap();

        UpdateInventoryUI(basic, basicText, levelData.basicInventory);
        UpdateInventoryUI(magnet, magnetText, levelData.magnetsInventory);
        UpdateInventoryUI(convey, conveyText, levelData.conveyorsInventory);
        UpdateInventoryUI(fan, fanText, levelData.fansInventory);
        UpdateInventoryUI(power, powerText, levelData.powersInventory);
        UpdateInventoryUI(pressFatigue, fatigueText, levelData.pressLimits, "Fatigue: ");
        UpdateInventoryUI(timeLimit, timeText, levelData.timeLimit, "Time: ");
    }

    private void InitializeInventoryMap()
    {
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

        if(levelData.difficulty == LevelData.difficultyLevel.easy)
        {
            difficultyColour.color = Color.green;
            difficultyText.text = "-Easy-";
        }
        else if(levelData.difficulty == LevelData.difficultyLevel.medium)
        {
            difficultyColour.color = Color.yellow;
            difficultyText.text = "-Medium-";
        }
        else if(levelData.difficulty == LevelData.difficultyLevel.hard)
        {
            difficultyColour.color = Color.red;
            difficultyText.text = "-Hard-";
        }

        descriptionText.text = levelData.levelDescription.ToString();
        if(levelData.externalThreat == "Cannon")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = cannonThreat;
            cannonText.text = "-Cannon-";
        }
        else if(levelData.externalThreat == "Laser")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = laserThreat;
            cannonText.text = "-Laser-";
        }
        else if(levelData.externalThreat == "Lasers")
        {
            externalThreat.SetActive(true);
            externalThreat.GetComponent<Image>().sprite = lasersThreat;
            cannonText.text = "-Lasers-";
        }
        else if(levelData.externalThreat == "ZeroGravity")
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

    private void UpdateInventoryUI(GameObject inventoryObject, TextMeshProUGUI inventoryText, float inventoryCount, string differenceText = "")
    {
        if (inventoryCount > 0)
        {
            inventoryObject.SetActive(true);
            inventoryText.text = $"-{differenceText}{inventoryCount.ToString("0")}-";
        }
        else
        {
            inventoryObject.SetActive(false);
        }
    }

    private void UpdateGameObjectState(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    public void PlayLevel()
    {
        levelSpawner.LoadLevel(level);
    }
}
