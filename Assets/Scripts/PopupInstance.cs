using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
