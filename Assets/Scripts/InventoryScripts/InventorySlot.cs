using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public TMPro.TextMeshProUGUI quantityText;

    Item item;
    Item.Items icon;

    private void Start()
    {
        item = GameObject.Find("Inventory").GetComponent<Item>();
    }

    public void AddItem(Item.Items newItem, int i)
    {
        icon = newItem;
        quantityText.text = i.ToString();
        image.sprite = item.GetItemObject(icon);
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
