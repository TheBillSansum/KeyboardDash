using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Script that runs all of the clippy hints and spawns them at specific locations depending on the hint number
/// <para> Some hints run other hints after but most are called from the level spawner</para>
/// </summary>
public class ClippyManager : MonoBehaviour
{
    public bool clippyHidden = false; //If the player has dismissed clippy
    public int currentHint;
    public string[] hints = new string[26]; //The text to display to the player
    public bool[] hintHeard = new bool[26]; //If that specific hint has already been heard in this playthrough
    public bool[] hintReady = new bool[26]; //If this hint should be played when the previous hint has been heard, most are false
    public Transform[] hintLocation = new Transform[26]; //Screen location of the menu 
    public Sprite[] clippySprite = new Sprite[26]; //Different faces of clippy to be

    public GameObject enableClippyButton; //Buttons for the menu
    public GameObject cullClippyButton;
    public GameObject clippyMenu;
    public bool clippyMenuActive = false;

    public GameObject turnOnSlowMode; //Special button that appears on the hint telling the player about slow mode, it is enabled as this reference

    public Image clippyImage;
    public TextMeshProUGUI clippyBody;
    public GameObject clippyObject;

    void Start()
    {
        hints[0] = "Hey, I'm Clippy! Your virtual assistant. I'm here to guide you through this game. If you already hate me, you can remove me by pressing 'Remove'!";
        hints[1] = "Here is the play area! The goal is to get the object on the keyboard into the goal, as shown by the gold!";
        hints[2] = "Use your keyboard to move the keys in the game!";
        hints[3] = "When you are ready, press 'Start'!";
        hints[4] = "Some levels have time limits! You must complete the goal before this time reaches 0, otherwise you will need to try again!";
        hints[5] = "You unlocked the inventory! Drag and drop from the slots and place keys on the desired key!";
        hints[6] = "The magnet key attracts any close metallic objects towards it. Press the key associated with it to use it!";
        hints[7] = "Some levels limit how many presses you can use per attempt. Once you hit the limit, no further presses can be tracked, but play continues!";
        hints[8] = "Goals that have a % require you to hold the key in the goal. Make sure to keep it contained, as if it leaves, progress is lost!";
        hints[9] = "Just like the last level, some levels contain multiple keys, which must all be completed at the same time to pass.";
        hints[10] = "You have 2 'Power' blocks this level and only one press. You will have to get creative in placements. Powering on the blocks doesn't count as a press ;)";
        hints[11] = "You unlocked a conveyor belt to place. You only have one though, so be cautious of where you place it!";
        hints[12] = "New item! Place the fans on the keyboard and then press that button during play to blow the closest key object away.";
        hints[13] = "Oh! The first 'Hard' level! Good luck, make sure to get a speedy start!";
        hints[14] = "Tut tut tut... Sticky keys! The groups of green keys all act as a group, keep that in mind!";
        hints[15] = "EXTERNAL DANGERS! Cannonballs will knock your key off-axis, make a wall from the bottom row to protect its journey.";
        hints[16] = "EXTERNAL DANGER! That laser will end your turn straight away. Don't get caught! At least there is only one...";
        hints[17] = "Ah... that's more than one. Okay, make sure to stay high to avoid those, one touch and it's over...";
        hints[18] = "Oooooo Zero Gravityyyyy, keep the key low otherwise you'll lose it!";
        hints[19] = "You messy slob, clean up this workspace! Think carefully about what keys you are pressing and the impact... you have 25 presses.";
        hints[20] = "Better cap that USB stick! Quick!";

        hints[25] = "Having issues? I can see you've failed this level a few times now... Did you know you can slow down time to allow more time for reaction speed? Head over to settings!";

        clippyMenu.SetActive(false); //Makes sure the menu is closed on start
        clippyMenuActive = false;

        PlayHint(0); //Starts the player off with the first hint
    }

    public void Update()
    {
        if (clippyHidden == true)
        {
            enableClippyButton.SetActive(true);
            cullClippyButton.SetActive(false);
        }
        if (clippyHidden == false)
        {      
            enableClippyButton.SetActive(false);
            cullClippyButton.SetActive(true);
        }
    }

    /// <summary>
    /// Function to play the specific hint, depending on the number it will spawn in particular locations
    /// </summary>
    /// <param name="hint"></param>
    public void PlayHint(int hint)
    {
        currentHint = hint;

        if (clippyHidden == false && hintHeard[hint]!= true) //Make sure it hasn't been heard and clippy isnt hidden
        {
            clippyObject.SetActive(true); 
            clippyObject.gameObject.transform.position = hintLocation[hint].position;
            clippyBody.text = hints[hint];
            hintHeard[hint] = true;
            
            if (hint == 25) //If the final hint which tells the player about 75% speed is called
            {
                turnOnSlowMode.SetActive(true); //Enable the special button
            }
            else //Remove it
            {
                turnOnSlowMode.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Functionality for the skip hint button on clippy
    /// </summary>
    public void SkipHint()
    {
        hintHeard[currentHint] = true;
        clippyObject.SetActive(false);

        if(hintReady[currentHint+1] == true)
        {
            PlayHint(currentHint+1);
        }
    }

    /// <summary>
    /// Functionality to cull clippy
    /// </summary>
    public void HideClippy()
    {
        clippyHidden = true;
        clippyObject.SetActive(false);
    }

    /// <summary>
    /// Functionality to bring Clippy back from being culled
    /// </summary>
    public void EnableClippy()
    {
        clippyHidden = false;
        clippyObject.SetActive(true);
    }

    /// <summary>
    /// Functionality to enable the clippy menu
    /// </summary>
    public void ToggleMenu()
    {
        if (clippyMenuActive)
        {
            clippyMenuActive = false;
            clippyMenu.SetActive(false);
        }
        else
        {
            clippyMenuActive = true;
            clippyMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Functionality to change Clippys emotions on his face, utilised in events
    /// <para> Options:</para>
    /// <para> "Normal",</para>
    /// <para> "Happy",</para>
    /// <para> "Sad",</para>
    /// <para> "Excited",</para>
    /// <para> "Petting",</para>
    /// </summary>
    /// <param name="emotion"></param>
    public void DisplayFace(string emotion)
    {
        switch(emotion)
        {
            case "Normal":
                clippyImage.sprite = clippySprite[0];
                break;

            case "Happy":
                clippyImage.sprite = clippySprite[1];
                break;

            case "Sad":
                clippyImage.sprite = clippySprite[2];
                break;

            case "Excited":
                clippyImage.sprite = clippySprite[3];
                break;

            case "Petting":
                clippyImage.sprite = clippySprite[4];
                break;
        }
    }

    /// <summary>
    /// Functionality completely reset clippy, option found in the clippy menu
    /// </summary>
    public void FactoryReset()
    {
        for (int i = 0; i < hintHeard.Length; i++) //Run through the array and clear them all
        {
            hintHeard[i] = false;
        }
        PlayHint(0); //Play the first hint again
    }
}
