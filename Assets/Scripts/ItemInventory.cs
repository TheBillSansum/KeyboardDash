using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInventory : MonoBehaviour
{
    public string objectTitle;
    public TextMeshProUGUI objectTitleText;
    public int quantity;
    public TextMeshProUGUI quanitityText;
    public bool inStock = false;
    public GameObject outOfStockSprite;


    void Update()
    {
        if (quantity >= 1)
        {
            inStock = true;
            outOfStockSprite.SetActive(false);
        }
        else
        {
            inStock = false;
            outOfStockSprite.SetActive(true);
        }

        quanitityText.text = "-"+quantity.ToString()+"-";
    }

    public void SetQuantity(int stock)
    {
        objectTitleText.text = objectTitle.ToString();
        quantity = stock;

    }
}
