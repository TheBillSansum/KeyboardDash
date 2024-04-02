using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClippyManager : MonoBehaviour
{
    public bool clippyHidden = false;
    public int currentHint;
    public string[] hints = new string[20];
    public bool[] hintHeard = new bool[20];
    public bool[] hintReady = new bool[20];
    public Transform[] hintLocation = new Transform[20];
    public Sprite[] clippySprite = new Sprite[20];

    public GameObject enableClippyButton;
    public GameObject cullClippyButton;
    public GameObject clippyMenu;
    public bool clippyMenuActive = false;

    public Image clippyImage;

    public TextMeshProUGUI clippyBody;
    public GameObject clippyObject;

    void Start()
    {
        hints[0] = "Hey, Im Clippy! Your virtual assistant, Im here to guide you through this Game, if you already hate me you can remove me by pressing 'Remove'!";
        hints[1] = "Here is the play area! The goal is to get the object on the keyboard into the Goal, as shown by the gold!";
        hints[2] = "Use your keyboard to move the keys in the Game!";
        hints[3] = "When you are ready, press 'Play'!";
        hints[4] = "Some levels have Time Limits! You must complete the goal before this time reaches 0 otherwise you will need to Try Again!";
        hints[5] = "You unlocked the Inventory! Drag and Drop from the slots and place keys on the desired key!";
        hints[6] = "The magnet key attracts any close metalic objects towards it, press the key assosiated with it to use it!";
        hints[7] = "Some levels limit how many presses you can use per attempt, once you hit the limit no further presses can be tracked, but play continues!";
        hints[8] = "Goals that have a % require you to hold the key in the goal, make sure to keep it contained as if it leaves progress is lost!";
        hints[9] = "Just like last level, some levels contain multiple keys, which must all be complete at the same time to pass";
        hints[10] = "You have 2 'Power' blocks this level and only one press, you will have to get creative in placements. Powering on the blocks don't count as a press ;)";
        hints[11] = "You unlocked a conveyer belt to place, you only have one though, be cautious of where you place it!";
        hints[12] = "New Item! Place the fans on the keyboard and then press that button during play to blow the closest key object away";
        hints[13] = "Oh! The first 'Hard' level! Good Luck, make sure to get a speedy start";

        clippyMenu.SetActive(false);
        clippyMenuActive = false;

        PlayHint(0);
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

    public void PlayHint(int hint)
    {
        Debug.Log("Instruction to play hint" + hint);
        currentHint = hint;

        if (clippyHidden == false && hintHeard[hint]!= true)
        {
            clippyObject.SetActive(true);
            clippyObject.gameObject.transform.position = hintLocation[hint].position;
            clippyBody.text = hints[hint];
            hintHeard[hint] = true;
        }
    }

    public void SkipHint()
    {
        hintHeard[currentHint] = true;
        clippyObject.SetActive(false);
        if(hintReady[currentHint+1] == true)
        {
            PlayHint(currentHint+1);
        }
    }

    public void HideClippy()
    {
        clippyHidden = true;
        clippyObject.SetActive(false);
    }
    
    public void EnableClippy()
    {
        clippyHidden = false;
        clippyObject.SetActive(true);
    }

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

    public void FactoryReset()
    {
        for (int i = 0; i < hintHeard.Length; i++)
        {
            hintHeard[i] = false;
        }
        PlayHint(0);
    }
}
