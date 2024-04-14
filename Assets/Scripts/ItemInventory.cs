using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Each draggable key has its own inventory, handles the quantity and removes ability to use if out of stock
/// </summary>
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
        if (quantity >= 1) //If any stock is left
        {
            inStock = true;
            outOfStockSprite.SetActive(false); //Remove the out of stock warning sign
        }
        else
        {
            inStock = false;
            outOfStockSprite.SetActive(true); //Add the out of stock warning sign which blocks the item being dragged from inventory
        }

        quanitityText.text = "-" + quantity.ToString() + "-"; //Display the quality, example "-5-"
    }

    /// <summary>
    /// Used in level loading 
    /// </summary>
    /// <param name="stock"></param>
    public void SetQuantity(int stock)
    {
        objectTitleText.text = objectTitle.ToString();
        quantity = stock;

    }
}
