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
    public int magnetsInventory;
    public int fansInventory;
}