using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only functionality is to ensure the pop up can be closed 
/// </summary>
public class PopupInstance : MonoBehaviour
{
    public void ClosePopup()
    {
        if (this != null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
