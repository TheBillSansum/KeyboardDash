using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClippyManager : MonoBehaviour
{
    public bool clippyHidden = false;
    public int currentHint;
    public string[] hints = new string[10];
    public bool[] hintHeard = new bool[10];
    public bool[] hintReady = new bool[10];
    public Transform[] hintLocation = new Transform[10];
    public Sprite[] clippySprite = new Sprite[10];

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



        PlayHint(0);
    }

    public void PlayHint(int hint)
    {
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

}
