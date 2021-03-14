using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    

    #region Inventory vars
    private static Dictionary<Item.Items, int> inventory;
    public static Inventory inv;
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
        inventory = new Dictionary<Item.Items, int>();
        UI = GetComponent<InventoryUI>();
    }
    #endregion

    #region Item Funcs
    public void AddItemToInventory(Item.Items item, int num)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += num;
        }
        else
        {
            inventory.Add(item, num);
        }
        UI.UpdateUI();
    }

    public Dictionary<Item.Items, int> GetInventory()
    {
        return inventory;
    }

    #endregion
}
