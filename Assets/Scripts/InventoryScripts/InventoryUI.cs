using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Inventory Variables
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
    private InventorySlot[] menuSlots;
    private InventorySlot[] barSlots;

    List<InventorySlot> menuList;

    private Inventory inventory;

    private bool inventoryOpened = false;

    public int NUM_SLOTS;
    #endregion

    #region Player Variables
    private GameObject player;
    private PlayerInteractions playerInteractions;
    private PlayerMovement playerMovement;
    #endregion

    #region Inventory Organization Variables
    private bool itemIsSelected;

    private int fromIndex;
    private int toIndex;

    public GameObject selectedItemUI;
    private RectTransform selectedItemRT;
    private Image selectedItemImage;

    private Item itemRef;
    #endregion

    #region Unity Functions 
    void Start()
    {
        inventory = Inventory.inv;

        //Set up UI
        menuSlots = inventoryMenu.GetComponentsInChildren<InventorySlot>();
        barSlots = inventoryBar.GetComponentsInChildren<InventorySlot>();
        menuList = new List<InventorySlot>();
        menuList.AddRange(menuSlots);
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        NUM_SLOTS = menuList.Count;

        player = GameObject.Find("Player");
        playerInteractions = player.GetComponent<PlayerInteractions>();
        playerMovement = player.GetComponent<PlayerMovement>();

        //Set up Inventory Organization
        itemIsSelected = false;
        selectedItemRT = selectedItemUI.GetComponent<RectTransform>();
        selectedItemImage = selectedItemUI.GetComponent<Image>();

        itemRef = GameObject.Find("Inventory").GetComponent<Item>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

    }
    private void FixedUpdate()
    {
        if (itemIsSelected)
        {
            FollowCurser();
        }
    }
    #endregion 

    #region Menu Functions
    public void OpenInventory()
    {
        itemIsSelected = false;
        inventoryOpened = true;
        inventoryMenu.SetActive(true);
        inventoryBar.SetActive(false);
        playerInteractions.SetMenuOpen(true);
        playerMovement.SetMenuOpen(true);

        UpdateUI();
    }

    public void CloseInventory()
    {
        selectedItemUI.SetActive(false);
        itemIsSelected = false;
        inventoryOpened = false;
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        playerInteractions.SetMenuOpen(false);
        playerMovement.SetMenuOpen(false);

        UpdateUI();
    }
    #endregion

    public void UpdateUI()
    {
       int i = 0;
       foreach (KeyValuePair<Item.Items, int> item in inventory.GetInventory()) {
           if (!inventoryOpened)
           {
               if (i >= barSlots.Length)
               {
                    break;
               }
               if (item.Key.Equals(Item.Items.empty))
               {
                    barSlots[i].RemoveItem();
               }
               else
               {
                    barSlots[i].AddItem(item.Key, item.Value);
               }
            }
           else
           {
                if (i >= menuSlots.Length)
                {
                    break;
                }
                if (item.Key.Equals(Item.Items.empty))
                {
                    menuSlots[i].RemoveItem();
                }
                else
                {
                    menuSlots[i].AddItem(item.Key, item.Value);
                }
            }   
           i++;
           }
    }

    #region Inventory Organization Functions
    private KeyValuePair<Item.Items, int> currentlySelected;
    public void OnClick(GameObject item)
    {
        if (inventoryOpened && itemIsSelected)
        {
            toIndex = GetIndexOfItem(item);
            KeyValuePair<Item.Items, int> previouslySelected = currentlySelected;
            currentlySelected = inventory.GetItemAtIndex(toIndex);

            //Debug.Log("from: " + fromIndex + " to: " + toIndex);
            inventory.MoveItemInInventory(fromIndex, toIndex, previouslySelected);
            SelectItem(currentlySelected.Key);

            if (currentlySelected.Key.Equals(Item.Items.empty))
            {
                Debug.Log("empty"); 
                selectedItemUI.SetActive(false);
                itemIsSelected = false;
            }

            UpdateUI();
        }
        else if (inventoryOpened && !itemIsSelected)
        {
            fromIndex = GetIndexOfItem(item);
            KeyValuePair<Item.Items, int> selected = inventory.GetItemAtIndex(fromIndex);
            if (!selected.Key.Equals(Item.Items.empty))
            {
                currentlySelected = selected;
                SelectItem(selected.Key);
            }
        }
        else
        {
            Debug.Log("uh oh... bug alert");
        }
    }


    private void SelectItem(Item.Items selected)
    {
        selectedItemImage.sprite = itemRef.GetItemSprite(selected);
        selectedItemRT.transform.position = Input.mousePosition;
        selectedItemUI.SetActive(true);

        menuList[fromIndex].RemoveItem();
        itemIsSelected = true;
    }

    private void FollowCurser()
    {
        Vector2 mousePos = Input.mousePosition;
        selectedItemRT.position = mousePos;
    }

    // Returns -1 if item is null
    private int GetIndexOfItem(GameObject item)
    {
        return menuList.IndexOf(item.GetComponent<InventorySlot>());
    }

    #endregion 
}
