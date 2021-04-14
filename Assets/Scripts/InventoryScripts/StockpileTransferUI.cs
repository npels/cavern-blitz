using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileTransferUI : MonoBehaviour {

    public GameObject inventoryObject;
    public GameObject stockpileObject;

    private Inventory playerInventory;
    private Inventory stockpileInventory;

    private List<InventorySlot> playerMenuSlots;
    private List<InventorySlot> stockpileSlots;


    private bool inventoryOpened = false;
    private bool itemIsSelected = false;

    public InventorySlot selectedItem;

    private void OnEnable() {
        playerInventory = BaseManager.instance.playerInventory;
        if (playerInventory.stacks == null) {
            stockpileInventory.InitInventory();
        }

        stockpileInventory = BaseManager.instance.stockpileInventory;
        if (stockpileInventory.stacks == null) {
            stockpileInventory.InitInventory();
        }

        inventoryObject.GetComponent<Inventory>().stacks = playerInventory.stacks;
        stockpileInventory.GetComponent<Inventory>().stacks = stockpileInventory.stacks;

        playerInventory.UpdateUIEvent += UpdateUI;
        stockpileInventory.UpdateUIEvent += UpdateUI;

        playerMenuSlots = new List<InventorySlot>();
        stockpileSlots = new List<InventorySlot>();

        InventorySlot[] slots = inventoryObject.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots) {
            playerMenuSlots.Add(slot);
        }

        slots = stockpileObject.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots) {
            stockpileSlots.Add(slot);
        }

        UpdateUI();
    }

    private void FixedUpdate() {
        if (itemIsSelected) {
            FollowCurser();
        }
    }

    public void OpenInventory() {
        itemIsSelected = false;
        inventoryOpened = true;
        gameObject.SetActive(true);
        stockpileInventory.LoadBaseInventory();
        playerInventory.SavePlayerInventory();

        UpdateUI();
    }

    public void CloseInventory() {
        if (itemIsSelected) {
            int index = playerInventory.TryAddItem(selectedItem.stack.item, selectedItem.stack.count);
            if (index == -1) {
                Destroy(selectedItem.itemObject);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            } else {
                InventorySlot slot = playerMenuSlots[index];
                Destroy(slot.itemObject);
                slot.itemObject = selectedItem.itemObject;
                slot.itemObject.transform.SetParent(slot.transform);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            }
        }
        itemIsSelected = false;
        inventoryOpened = false;
        gameObject.SetActive(false);
        stockpileInventory.SaveBaseInventory();
        playerInventory.SavePlayerInventory();

        UpdateUI();
    }

    public void UpdateUI() {
        for (int i = 0; i < playerInventory.stacks.Count; i++) {
            playerMenuSlots[i].UpdateSlot(playerInventory.stacks[i]);
        }

        for (int i = 0; i < stockpileInventory.stacks.Count; i++) {
            stockpileSlots[i].UpdateSlot(stockpileInventory.stacks[i]);
        }
    }

    #region Inventory Organization Functions
    public void OnClick(GameObject item) {
        if (inventoryOpened && itemIsSelected) {
            PutSelectedItem(item);

            UpdateUI();
        } else if (inventoryOpened && !itemIsSelected) {
            PickupItem(item);

            UpdateUI();
        }
    }

    private void PutSelectedItem(GameObject item) {
        int index = GetIndexOfItem(item);
        bool inPlayerInventory = playerMenuSlots.Contains(item.GetComponent<InventorySlot>());
        InventorySlot slot = inPlayerInventory ? playerMenuSlots[index] : stockpileSlots[index];
        ItemStack stack = slot.stack;
        GameObject obj = slot.itemObject;

        if (stack.item == null) {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = null;
            selectedItem.itemObject = null;

            if (inPlayerInventory) playerInventory.SetItemStack(index, slot.stack);
            else stockpileInventory.TryAddItem(slot.stack.item, slot.stack.count, false);

            slot.itemObject.transform.SetParent(slot.transform);

            itemIsSelected = false;
        } else {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = stack;
            selectedItem.itemObject = obj;

            if (inPlayerInventory) playerInventory.SetItemStack(index, slot.stack);
            else stockpileInventory.TryAddItem(slot.stack.item, slot.stack.count, false);

            slot.itemObject.transform.SetParent(slot.transform);
            selectedItem.itemObject.transform.SetParent(inventoryObject.transform);

            itemIsSelected = true;
        }
    }

    private void PickupItem(GameObject item) {
        int index = GetIndexOfItem(item);
        bool inPlayerInventory = playerMenuSlots.Contains(item.GetComponent<InventorySlot>());
        InventorySlot slot = inPlayerInventory ? playerMenuSlots[index] : stockpileSlots[index];

        if (slot.stack.item != null) {
            selectedItem.stack = slot.stack;
            selectedItem.itemObject = slot.itemObject;

            slot.stack = new ItemStack(!inPlayerInventory);
            slot.itemObject = null;

            if (inPlayerInventory) playerInventory.SetItemStack(index, slot.stack);
            else stockpileInventory.SetItemStack(index, slot.stack);

            selectedItem.itemObject.transform.SetParent(selectedItem.transform);

            itemIsSelected = true;
        } else {
            selectedItem.stack = null;
            selectedItem.itemObject = null;

            itemIsSelected = false;
        }
    }

    private void FollowCurser() {
        Vector2 mousePos = Input.mousePosition;
        selectedItem.itemObject.transform.position = mousePos;
    }

    private int GetIndexOfItem(GameObject item) {
        int result = playerMenuSlots.IndexOf(item.GetComponent<InventorySlot>());
        if (result >= 0) {
            return result;
        } else {
            return stockpileSlots.IndexOf(item.GetComponent<InventorySlot>());
        }
    }

    #endregion 
}
