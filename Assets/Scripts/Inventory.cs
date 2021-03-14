using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    #region UI Vars
    private GameObject inventoryBar;
    private GameObject inventoryMenu;
    private GameObject openButton;
    private GameObject closeButton;
    #endregion

    #region Unity Funcs
    // Start is called before the first frame update
    void Start()
    {
        //Set up UI
        inventoryBar = GameObject.Find("InventoryBar");
        inventoryMenu = GameObject.Find("InventoryMenu");
        openButton = GameObject.Find("OpenInventory");
        closeButton = GameObject.Find("CloseInventory");
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
}
