using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileInventoryUI : MonoBehaviour {

    private Inventory inventory;
    private InventorySlot[] menuSlots;

    private void OnEnable() {
        inventory = BaseManager.instance.stockpileInventory;
        if (inventory.stacks == null) {
            inventory.InitInventory();
        }
        inventory.LoadBaseInventory();
        inventory.UpdateUIEvent += UpdateUI;

        menuSlots = GetComponentsInChildren<InventorySlot>();

        UpdateUI();
    }

    private void OnDisable() {
        inventory.SaveBaseInventory();
        inventory.UpdateUIEvent -= UpdateUI;
    }

    public void UpdateUI() {
        for (int i = 0; i < inventory.stacks.Count; i++) {
            menuSlots[i].UpdateSlot(inventory.stacks[i]);
        }
    }
}
