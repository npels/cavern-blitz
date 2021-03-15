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
        menuSlots = inventoryMenu.GetComponentsInChildren<InventorySlot>();
        barSlots = inventoryBar.GetComponentsInChildren<InventorySlot>();
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }



    public void OpenMenu()
    {
        inMenu = true;
        inventoryMenu.SetActive(true);
        inventoryBar.SetActive(false);
        openButton.SetActive(false);
        closeButton.SetActive(true);

        UpdateUI();
    }

    public void CloseMenu()
    {
        inMenu = false;
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);

        UpdateUI();
    }

    public void UpdateUI()
    {
       int i = 0;
       foreach (KeyValuePair<Item.Items, int> item in inventory.GetInventory()) {
           if (!inMenu)
           {
               if (i >= barSlots.Length)
               {
                    break;
               }
                barSlots[i].AddItem(item.Key, item.Value);
           }
           else
           {
                if (i >= menuSlots.Length)
                {
                    break;
                }
                menuSlots[i].AddItem(item.Key, item.Value);
           }   
           i++;
           }
        
    }
}
