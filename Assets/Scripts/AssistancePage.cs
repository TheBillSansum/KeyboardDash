using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistancePage : MonoBehaviour
{
    public GameObject[] buttonMask = new GameObject[3];
    public GameObject[] sliderContent = new GameObject[3];


    public void Start()
    {
        LoadContent(0);
    }

    public void LoadContent(int num)
    {
        foreach (GameObject buttonmask in buttonMask)
        {
            buttonmask.SetActive(false);
        }
        foreach(GameObject slidercontent in sliderContent)
        {
            slidercontent.SetActive(false);
        }
        buttonMask[num].SetActive(true);
        sliderContent[num].SetActive(true);
    }

    public void HideScreen()
    {
        this.gameObject.SetActive(false);
    }


}
