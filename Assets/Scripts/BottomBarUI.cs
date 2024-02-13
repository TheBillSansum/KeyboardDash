using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BottomBarUI : MonoBehaviour
{
    public TMP_Text timeText;

    public bool enlargeStart = false;
    public GameObject enlargeStartObject;

    public bool enlargeHelp = false;
    public GameObject enlargeHelpObject;

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

    public void Update()
    {
        if (enlargeStart)
        {
            enlargeStartObject.SetActive(true);
        }
        else
        {
            enlargeStartObject.SetActive(false);
        }
    }

    public void ToggleStart()
    {
        if (enlargeStart == false)
        {
            enlargeStartObject.SetActive(true);
            enlargeStart = true;
        }
        else
        {
            enlargeStartObject.SetActive(false);
            enlargeStart = false;
        }
    }

    public void ToggleHelp()
    {
        if (enlargeHelp == false)
        {
            enlargeHelpObject.SetActive(true);
            enlargeHelp = true;
        }
        else
        {
            enlargeHelpObject.SetActive(false);
            enlargeHelp = false;
        }
    }
}

