using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    

    #region Inventory vars
    private static Dictionary<Item.Items, int> inventory1;
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
        inventory1 = new Dictionary<Item.Items, int>();
        inventory = new List<KeyValuePair<Item.Items, int>>();
        UI = GetComponent<InventoryUI>();
        maxItemSize = 64;
    }
    #endregion

    #region Item Funcs
    public void AddItemToInventory(Item.Items item, int num)
    {
        /*if (num > 0)
        {
            if (inventory1.ContainsKey(item) && inventory1[item] <= 2 - num)
            {
                inventory1[item] += num;
            }
            else
            {
                inventory1.Add(item, num);
            }
            UI.UpdateUI();
        }*/

        if (num > 0)
        {
            int size = inventory.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {

                    if (inventory[i].Key == item && inventory[i].Value <= maxItemSize - num)
                    {
                        inventory[i] = new KeyValuePair<Item.Items, int>(item, inventory[i].Value + num);
                        break;
                    }
                    if (i == size - 1)
                    {
                        inventory.Add(new KeyValuePair<Item.Items, int>(item, num));
                    }
                }
            } else
            {
                inventory.Add(new KeyValuePair<Item.Items, int>(item, num));
            }
            UI.UpdateUI();
        }

    }

    public List<KeyValuePair<Item.Items, int>> GetInventory()
    {
        return inventory;
    }

    #endregion
}
