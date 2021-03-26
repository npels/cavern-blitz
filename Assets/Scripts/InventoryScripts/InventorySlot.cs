﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public TMPro.TextMeshProUGUI quantityText;

    Item item;

    private void Awake()
    {
        item = GameObject.Find("Inventory").GetComponent<Item>();
        
    }

    public void AddItem(Item.Items newItem, int i)
    {
        quantityText.enabled = true;
        image.enabled = true;

        quantityText.text = i.ToString();
        image.sprite = item.GetItemSprite(newItem);
        image.enabled = true;
        quantityText.enabled = true;
    }

    public void RemoveItem()
    {
        quantityText.enabled = false;
        image.enabled = false;
    }

    public void Moving()
    {
        image.enabled = true;
        quantityText.enabled = false;
    }

}