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
    public Sprite[] clippySprite = new Sprite[10];

    public TextMeshProUGUI clippyBody;
    public GameObject clippyObject;

    void Start()
    {
        hints[0] = "Hey, Im Clippy! Your virtual assistant, Im here to guide you through this Game, if you already hate me you can remove me by pressing 'Remove'!";


        clippyBody.text = hints[0];



    }

    void Update()
    {
        
    }
}
