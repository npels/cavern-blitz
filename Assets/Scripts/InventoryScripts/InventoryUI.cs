using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
    public GameObject openButton;
    public GameObject closeButton;

    private Inventory inventory;

    private bool inMenu = false;
    private InventorySlot[] menuSlots;
    private InventorySlot[] barSlots;

    void Start()
    {
        inventory = Inventory.inv;
        //Set up UI
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);

        menuSlots = inventoryMenu.GetComponentsInChildren<InventorySlot>();
        barSlots = inventoryBar.GetComponentsInChildren<InventorySlot>();
    }



    public void OpenMenu()
    {
        inMenu = true;
        inventoryBar.SetActive(false);
        inventoryMenu.SetActive(true);
        openButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void CloseMenu()
    {
        inMenu = false;
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }

    public void UpdateUI()
    {
        Debug.Log("update UI");
        if (inMenu)
        {

        } else
        {
            int i = 0;
            foreach (KeyValuePair<Item.Items, int> item in inventory.GetInventory()) {
                if (i >= barSlots.Length)
                {
                    break;
                }
                barSlots[i].AddItem(item.Key, item.Value);
                i++;
            }
        }
    }
}
