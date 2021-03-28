using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    #region Inventory Variables
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
    private InventorySlot[] menuSlots;
    private InventorySlot[] barSlots;

    List<InventorySlot> menuList;

    private Inventory inventory;

    private bool inventoryOpened = false;
    #endregion

    #region Player Variables
    private GameObject player;
    private PlayerInteractions playerInteractions;
    private PlayerMovement playerMovement;
    #endregion

    #region Inventory Organization Variables
    private bool itemIsSelected;
    public InventorySlot selectedItem;
    #endregion

    #region Unity Functions 
    void Start() {
        inventory = GameManager.instance.inventory;
        inventory.UpdateUIEvent += UpdateUI;

        //Set up UI
        menuSlots = inventoryMenu.GetComponentsInChildren<InventorySlot>();
        barSlots = inventoryBar.GetComponentsInChildren<InventorySlot>();
        menuList = new List<InventorySlot>();
        menuList.AddRange(menuSlots);
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);

        player = GameManager.instance.player;
        playerInteractions = player.GetComponent<PlayerInteractions>();
        playerMovement = player.GetComponent<PlayerMovement>();

        //Set up Inventory Organization
        itemIsSelected = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (inventoryOpened) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }
    private void FixedUpdate() {
        if (itemIsSelected) {
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
        if (itemIsSelected) {
            int index = inventory.TryAddItem(selectedItem.stack.item, selectedItem.stack.count);
            if (index == -1) {
                Destroy(selectedItem.itemObject);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            } else {
                InventorySlot slot = menuSlots[index];
                slot.itemObject = selectedItem.itemObject;
                selectedItem.itemObject.transform.SetParent(slot.transform);
            }
        }
        itemIsSelected = false;
        inventoryOpened = false;
        inventoryBar.SetActive(true);
        inventoryMenu.SetActive(false);
        playerInteractions.SetMenuOpen(false);
        playerMovement.SetMenuOpen(false);

        UpdateUI();
    }
    #endregion

    public void UpdateUI() {
        for (int i = 0; i < inventory.stacks.Count; i++) {
            menuSlots[i].UpdateSlot(inventory.stacks[i]);
        }

        for (int i = 0; i < inventory.numPrioritySlots; i++) {
            barSlots[i].UpdateSlot(inventory.stacks[inventory.GetPriorityIndex(i)]);
        }
    }

    #region Inventory Organization Functions
    public void OnClick(GameObject item) {
        if (inventoryOpened && itemIsSelected) {
            PutSelectedItem(item);

            UpdateUI();
        }
        else if (inventoryOpened && !itemIsSelected) {
            PickupItem(item);

            UpdateUI();
        }
    }

    private void PutSelectedItem(GameObject item) {
        int index = GetIndexOfItem(item);
        InventorySlot slot = menuSlots[index];
        ItemStack stack = slot.stack;
        GameObject obj = slot.itemObject;

        if (stack.item == null) {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = null;
            selectedItem.itemObject = null;

            inventory.SetItemStack(index, slot.stack);

            slot.itemObject.transform.SetParent(slot.transform);

            itemIsSelected = false;
        } else {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = stack;
            selectedItem.itemObject = obj;

            inventory.SetItemStack(index, slot.stack);

            slot.itemObject.transform.SetParent(slot.transform);
            selectedItem.itemObject.transform.SetParent(inventoryMenu.transform);
            
            itemIsSelected = true;
        }
    }

    private void PickupItem(GameObject item) {
        int index = GetIndexOfItem(item);
        InventorySlot slot = menuSlots[index];

        if (slot.stack.item != null) {
            selectedItem.stack = slot.stack;
            selectedItem.itemObject = slot.itemObject;

            slot.stack = new ItemStack(inventory.allowStockpile);
            slot.itemObject = null;

            inventory.SetItemStack(index, slot.stack);

            selectedItem.itemObject.transform.SetParent(inventoryMenu.transform);

            itemIsSelected = true;
        } else {
            selectedItem.stack = null;
            selectedItem.itemObject = null;

            itemIsSelected = false;
        }
    }

    private void FollowCurser()
    {
        Vector2 mousePos = Input.mousePosition;
        selectedItem.itemObject.transform.position = mousePos;
    }

    // Returns -1 if item is null
    private int GetIndexOfItem(GameObject item)
    {
        return menuList.IndexOf(item.GetComponent<InventorySlot>());
    }

    #endregion 
}
