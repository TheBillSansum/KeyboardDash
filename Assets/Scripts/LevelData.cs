using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Levels/Level")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public string levelName;
    public string levelDescription;
    public GameObject sceneObject;
    public GameObject keyboardEvents;
    public GameObject transparentStartPoint;
    public bool inventoryActive = true;
    public float timeLimit;
    public float pressLimits;
    public int basicInventory;  
    public int magnetsInventory;
    public int fansInventory;
    public int conveyorsInventory;
    public int powersInventory;
    public difficultyLevel difficulty;
    public float personalRecord;
    public string externalThreat;
    public enum difficultyLevel 
    {
        easy,
        medium, 
        hard
    }
}