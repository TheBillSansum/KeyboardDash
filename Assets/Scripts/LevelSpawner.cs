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

    public LevelData[] levelData;

    void Start()
    {
        LoadLevel(0);
    }


    public void LoadLevel(int level)
    {
        Instantiate(levelData[level].sceneObject);
        basicInventory.SetQuantity(levelData[level].basicInventory);
        fanInventory.SetQuantity(levelData[level].fansInventory);
        magnetInventory.SetQuantity(levelData[level].magnetsInventory);
        pushInventory.SetQuantity(levelData[level].conveyorsInventory);
    }




}
