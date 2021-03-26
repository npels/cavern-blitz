using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    

    #region Inventory vars
    private static List<KeyValuePair<Item.Items, int>>inventory;
    public static Inventory inv;
    private static int maxItemSize;
    #endregion

    InventoryUI UI; 

    #region Unity Funcs
    // Start is called before the first frame update
    private void Awake()
    {
        if (inv != null)
        {
            Debug.Log("More than one inventory found");
            return;
        }
        inv = this;
    }

    void Start()
    {
        inventory = new List<KeyValuePair<Item.Items, int>>();
        UI = GetComponent<InventoryUI>();
        maxItemSize = 64;

        for (int i = 0; i < UI.NUM_SLOTS; i ++)
        {
            inventory.Add(new KeyValuePair<Item.Items, int>(Item.Items.empty, 0));
        }
    }
    #endregion

    #region Item Funcs
    public void AddItemToInventory(Item.Items item, int num)
    {
        if (num > 0)
        {
            int size = inventory.Count;
            int indexToAdd = 0;
            for (int i = 0; i < size; i++)
            {
                if ((inventory[i].Key == item && inventory[i].Value <= maxItemSize - num) || inventory[i].Key == Item.Items.empty)
                {
                    indexToAdd = i;
                    break;
                } 
            }
            inventory[indexToAdd] = new KeyValuePair<Item.Items, int>(item, inventory[indexToAdd].Value + num);
            UI.UpdateUI();
        }

    }

    
    #endregion

    #region Get/Set Funcs
    public List<KeyValuePair<Item.Items, int>> GetInventory()
    {
        return inventory;
    }
    public KeyValuePair<Item.Items, int> GetItemAtIndex(int index)
    {
        return inventory[index];
    }

    public void MoveItemInInventory(int fromIndex, int toIndex, KeyValuePair<Item.Items, int> item)
    {
        if (fromIndex >= inventory.Count || toIndex >= inventory.Count)
        {
            return;
        }
        inventory[fromIndex] = new KeyValuePair<Item.Items, int>(Item.Items.empty, 0);
        inventory[toIndex] = item;
    }

    #endregion
}
