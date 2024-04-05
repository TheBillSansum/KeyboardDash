using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject slowModeEnableButton;
    public GameObject slowModeDisableButton;

    public GameObject musicToggleEnable;
    public GameObject musicToggleDisable;

    public GameObject musicObject;

    void Update()
    {
        if (PlayerPrefs.GetInt("SlowMode") == 1)
        {
            slowModeEnableButton.SetActive(false);
            slowModeDisableButton.SetActive(true);
        }
        else
        {
            slowModeEnableButton.SetActive(true);
            slowModeDisableButton.SetActive(false);
        }

        if (PlayerPrefs.GetInt("MusicOff") == 1)
        {
            musicToggleEnable.SetActive(true);
            musicToggleDisable.SetActive(false);
            musicObject.SetActive(false);
        }

        if(PlayerPrefs.GetInt("MusicOff") == 0)
        {
            musicToggleEnable.SetActive(false);
            musicToggleDisable.SetActive(true);
            musicObject.SetActive(true);
        }
    }



    public void ButtonPress(string Action_TrueFalse)
    {
        switch (Action_TrueFalse)
        {
            case ("Slow_True"):
                PlayerPrefs.SetInt("SlowMode", 1);
                break;

            case ("Slow_False"):
                PlayerPrefs.SetInt("SlowMode", 0);
                break;

            case ("Music_True"):
                PlayerPrefs.SetInt("MusicOff", 0);
                break;

            case ("Music_False"):
                PlayerPrefs.SetInt("MusicOff", 1);
                break;
        }

        PlayerPrefs.Save();
    }
}
