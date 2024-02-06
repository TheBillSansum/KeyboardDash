using System;
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnglishKeyboardLayout", menuName = "Custom/Keyboard Layout/English")]
public class EnglishKeyboardLayout : ScriptableObject
{
    [Header("Alphabets and Numbers")]
   [SerializeField] public GameObject prefab;

    // A list of transforms representing the positions for each key
    public Transform[] keyTransforms;

    // Get the transform associated with a specific key
    public Transform GetTransform(char key)
    {
        int index = (int)key - (int)'A'; // Assuming keys are A-Z
        if (index >= 0 && index < keyTransforms.Length)
        {
            return keyTransforms[index];
        }
        else if (Char.IsDigit(key))
        {
            // Handle numbers (0-9)
            int number = (int)Char.GetNumericValue(key);
            if (number >= 0 && number <= 9 && number < keyTransforms.Length)
            {
                return keyTransforms[number];
            }
        }

        Debug.LogError("Invalid key or key not found in the list.");
        return null;
    }
}