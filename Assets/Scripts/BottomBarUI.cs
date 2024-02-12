using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottomBarUI : MonoBehaviour
{
    public TMP_Text timeText;

    void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 60f);
    }

    void UpdateTime()
    {
        System.DateTime currentTime = System.DateTime.Now;

        string timeString = currentTime.ToString("hh:mm tt").ToUpper();


        timeText.text = timeString;
    }
}

