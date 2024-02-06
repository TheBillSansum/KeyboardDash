using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyboardLayout", menuName = "Custom/Keyboard Layout")]
public class KeyboardLayout : ScriptableObject
{
    public Dictionary<char, GameObject> keyMappings = new Dictionary<char, GameObject>();
}
