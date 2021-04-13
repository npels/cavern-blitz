using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileInventoryUI : MonoBehaviour {

    private Inventory inventory;
    private InventorySlot[] menuSlots;

    private void Start() {
        inventory = CraftingManager.instance.stockpileInventory;
        inventory.UpdateUIEvent += UpdateUI;

        menuSlots = GetComponentsInChildren<InventorySlot>();
    }

    public void UpdateUI() {
        for (int i = 0; i < inventory.stacks.Count; i++) {
            menuSlots[i].UpdateSlot(inventory.stacks[i]);
        }
    }
}
