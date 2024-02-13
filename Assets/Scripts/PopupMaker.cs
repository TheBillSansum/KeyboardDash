using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupMaker : MonoBehaviour
{
    public GameObject popUpObjectBlank;
    public TMP_Text popUpTitle;
    public TMP_Text popUpBody;
    public Image popUpImage;

    public Sprite warningSprite;
    public Sprite errorSprite;

    public void Generate(string Title, string Body, string Image)
    {
        popUpObjectBlank.SetActive(true);
        popUpTitle.text = Title;
        popUpBody.text = Body;

        if (Image == "Warning")
        {
            popUpImage.sprite = warningSprite;

        }
        else if (Image == "Error")
        {
            popUpImage.sprite = errorSprite;
        }


    }



    public void Start()
    {
        Generate("Error 404", "Skill Not Found", "Warning");
    }
}
