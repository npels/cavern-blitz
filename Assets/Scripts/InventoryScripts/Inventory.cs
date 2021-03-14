using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    #region UI Vars
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
    public GameObject openButton;
    public GameObject closeButton;
    #endregion

    #region Inventory vars
    private static Dictionary<Item.Items, int> inventory;
    public static Inventory inv;
    #endregion

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

        //Set up UI
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }
    #endregion


    #region UI funcs
    public void OpenMenu()
    {
        inventoryBar.SetActive(false);
        inventoryMenu.SetActive(true);
        openButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void CloseMenu()
    {
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);
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
    }
    #endregion
}
