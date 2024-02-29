using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSpawner : MonoBehaviour
{
    public ItemInventory basicInventory;
    public ItemInventory fanInventory;
    public ItemInventory magnetInventory;
    public ItemInventory pushInventory;
    public ItemInventory powerInventory;

    public LevelData[] levelData;
    public GameObject levelObject;

    public TextMeshProUGUI title;


    void Start()
    {
        LoadLevel(0);
    }


    public void LoadLevel(int level)
    {
        levelObject = Instantiate(levelData[level].sceneObject);
        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
        powerInventory.SetQuantity(levelData[level].powersInventory);

        title.text = "Level " + levelData[level].levelNumber.ToString() + " - " + levelData[level].levelName;
    }

    public void ResetLevel()
    {
        levelObject.gameObject.SetActive(false);
        LoadLevel(0);
    }




}
