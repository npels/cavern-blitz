using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    

    #region Inventory vars
    private static List<KeyValuePair<Item.Items, int>> inventory;
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
            int indexToAdd = -1;
            int firstEmpty = -1;
            for (int i = 0; i < size; i++)
            {
                if ((inventory[i].Key == item && inventory[i].Value <= maxItemSize - num))
                {
                    indexToAdd = i;
                    PutItemAtIndex(indexToAdd, new KeyValuePair<Item.Items, int>(item, inventory[indexToAdd].Value + num));
                    break;
                }
                else if (inventory[i].Key == Item.Items.empty && firstEmpty == -1)
                {
                    firstEmpty = i;
                }
            }
            if (indexToAdd == -1)
            {
                PutItemAtIndex(firstEmpty, new KeyValuePair<Item.Items, int>(item, num));

            }
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
        if (index >= inventory.Count)
        {
            Debug.LogError("Index is out of range");
            return new KeyValuePair<Item.Items, int>(Item.Items.empty, 0);
        }
        return inventory[index];
    }
    public void PutItemAtIndex(int index, KeyValuePair<Item.Items, int> item)
    {
        if (index >= inventory.Count)
        {
            Debug.LogError("Index is out of range");
            return;
        }
        inventory[index] = item;
    }

    public void MoveItemInInventory(int fromIndex, int toIndex, KeyValuePair<Item.Items, int> item)
    {
        PutItemAtIndex(fromIndex, new KeyValuePair<Item.Items, int>(Item.Items.empty, 0));
        PutItemAtIndex(toIndex, item);
    }

    #endregion
}
