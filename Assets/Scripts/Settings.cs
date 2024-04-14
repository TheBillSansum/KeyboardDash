using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for functionality of the settings menu
/// </summary>
public class Settings : MonoBehaviour
{
    //Only one button is active at any time, per setting to not confuse the player

    public GameObject slowModeEnableButton; //Button to ENABLE slow mode 
    public GameObject slowModeDisableButton; //Button to DISABLE slow mode

    public GameObject musicToggleEnable; //Button to ENABLE music
    public GameObject musicToggleDisable; //Button to DISABLE music

    public GameObject musicObject; //The UI object for disabling

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


    /// <summary>
    /// Reuseable script to change different settings utilising the same script with different strings
    /// <para>Options:</para>
    /// <para>"Slow_True", "Slow_False", "Music_True", "Music_False"</para>
    /// </summary>
    /// <param name="Action_TrueFalse"></param>
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
