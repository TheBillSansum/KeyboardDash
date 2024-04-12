using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functionality for the 'Windows Assistance Page'
/// </summary>

public class AssistancePage : MonoBehaviour
{
    public GameObject[] buttonMask = new GameObject[3];
    public GameObject[] sliderContent = new GameObject[3];


    public void Start()
    {
        LoadContent(0);
    }

    /// <summary>
    /// Load different content on help page
    /// <para> 0 = General</para>
    /// <para> 1 = Keys</para>
    /// <para> 2 = Coming Soon!</para>
    /// </summary>
    /// <param name="num"></param>
    public void LoadContent(int num)
    {
        foreach (GameObject buttonmask in buttonMask)
        {
            buttonmask.SetActive(false); //Removes section at the top of the button to have a seamless section
        }
        foreach(GameObject slidercontent in sliderContent) //Remove all content from 'content' section of scroll view
        {
            slidercontent.SetActive(false);
        }
        buttonMask[num].SetActive(true); //Only enable the selected sections mask
        sliderContent[num].SetActive(true); //Only enable the selected sections content
    }


    /// <summary>
    /// Remove the 'Windows Assistant Page' object
    /// </summary>
    public void HideScreen()
    {
        this.gameObject.SetActive(false);
    }


}
