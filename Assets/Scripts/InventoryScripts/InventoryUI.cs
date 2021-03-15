using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
   
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inMenu)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        inMenu = true;
        inventoryMenu.SetActive(true);
        inventoryBar.SetActive(false);

        UpdateUI();
    }

    public void CloseMenu()
    {
        inMenu = false;
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);

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
