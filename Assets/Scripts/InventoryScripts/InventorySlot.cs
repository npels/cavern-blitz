using System.Collections;
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
        quantityText.text = i.ToString();
        image.sprite = item.GetItemObject(newItem);
        image.enabled = true;
        quantityText.enabled = true;
    }

    public void RemoveItem()
    {
        image = null;
        quantityText = null;
        image.sprite = null;
        image.enabled = false;
        quantityText.enabled = true;
    }

}
