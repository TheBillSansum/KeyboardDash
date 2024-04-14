using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
/// <summary>
/// Scripting for the functionality of the bottom bar UI and its buttons
/// </summary>
public class BottomBarUI : MonoBehaviour
{
    public TMP_Text timeText; //The clock text on the bottom right of screen

    public bool enlargeStart = false; //If the start menu is extended
    public GameObject enlargeStartObject; //The start menu for activating or deactivating

    public bool enlargeHelp = false; //If the 'Windows Assistance Page' is up
    public GameObject enlargeHelpObject; //'Windows Assistance Page' object for activating or deactivating

    void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 60f);
    }

    void UpdateTime()
    {
        System.DateTime currentTime = System.DateTime.Now;

        string timeString = currentTime.ToString("hh:mm tt").ToUpper(); //Display the text with the correct format

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

    /// <summary>
    ///  //Opens the feedback google form when pressing the button
    ///  <para>https://forms.gle/R8za8bGTmkRmVC5t9</para>
    /// </summary>
    public void OpenFeedback()
    {
        Application.OpenURL("https://forms.gle/R8za8bGTmkRmVC5t9");
    }

    /// <summary>
    /// Closes the artefact
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Reloads the first scene
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Toggles the extendable start menu
    /// </summary>
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

    /// <summary>
    /// Toggles between the help page being up or down
    /// </summary>
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