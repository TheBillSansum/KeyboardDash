using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI basicText;
    public TextMeshProUGUI magnetText;
    public TextMeshProUGUI conveyText;
    public TextMeshProUGUI fanText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI fatigueText;
    public TextMeshProUGUI timeText;

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
    }

    private void UpdateInventoryUI(GameObject inventoryObject, TextMeshProUGUI inventoryText, float inventoryCount, string differenceText = "")
    {
        if (inventoryCount > 1)
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
