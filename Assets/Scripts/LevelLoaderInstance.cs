using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLoaderInstance : MonoBehaviour
{
    public int level;
    public LevelSpawner levelSpawner;
    public LevelData leveldata;

    public TextMeshProUGUI levelText;
    public GameObject basic;
    public GameObject magnet;
    public GameObject convey;
    public GameObject fan;
    public GameObject power;

    void Start()
    {
        leveldata = levelSpawner.levelData[level];
        levelText.text = "Level " + level + " - " + leveldata.levelName;

        if (leveldata.basicInventory > 1)
        {
            basic.SetActive(true);
        }
        if (leveldata.magnetsInventory > 1)
        {
            magnet.SetActive(true);
        }
        if(leveldata.conveyorsInventory > 1)
        {
            convey.SetActive(true);
        }
        if(leveldata.fansInventory > 1)
        {
            fan.SetActive(true);
        }
        if(leveldata.powersInventory > 1)
        {
            power.SetActive(true);
        }
    }

    public void PlayLevel()
    {
        levelSpawner.LoadLevel(level);
    }
}
