using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory : MonoBehaviour {

    #region Inventory parameters
    [Tooltip("The number of slots this inventory has.")]
    public int numSlots;
    [Tooltip("If true, the maximum stack sizes of items are ignored.")]
    public bool allowStockpile;
    #endregion

    #region Inventory variables
    public List<ItemStack> stacks;
    #endregion

    #region Event declarations
    public delegate void UpdateUIFunction();
    public event UpdateUIFunction UpdateUIEvent;

    /* Update any UI objects that are following this inventory. */
    public void UpdateUI() {
        UpdateUIEvent?.Invoke();
    }
    #endregion

    #region Static variables
    public static List<ItemStack> savedPlayerInventory;
    public static List<ItemStack> savedBaseInventory;
    #endregion

    #region Unity functions
    void Awake() {
        InitInventory();
    }
    #endregion

    public void InitInventory() {
        stacks = new List<ItemStack>();

        for (int i = 0; i < numSlots; i++) {
            stacks.Add(new ItemStack(allowStockpile));
        }
    }


    #region Item Funcs
    /* Tries to add the item to the inventory.
     * If there is no space, return -1. 
     * Otherwise, add the item and return the index of the slot. */
    public int TryAddItem(Item item, int count, bool updateUI = true) {
        if (count <= 0) return -1;
        if (item == null) return -1;

        ItemStack stack = GetItemStack(item);
        if (stack != null) {
            if (stack.TryStackItem(item, count)) {
                if (updateUI) UpdateUI();
                return stacks.IndexOf(stack);
            }
        }

        foreach (ItemStack s in stacks) {
            if (s.TryStackItem(item, count)) {
                if (updateUI) UpdateUI();
                return stacks.IndexOf(s);
            }
        }

        return -1;
    }

    /* Returns the ItemStack at slot 'slotIndex'. */
    public ItemStack GetItemStack(int slotIndex) {
        if (slotIndex >= numSlots) {
            Debug.LogError("Tried to access item at slot " + slotIndex + " when the max slot index is " + (numSlots - 1));
            return null;
        }

        return stacks[slotIndex];
    }

    /* Returns the first stack of 'item' in the inventory, or null if there are none. */
    public ItemStack GetItemStack(Item item) {
        foreach (ItemStack stack in stacks) {
            if (stack.item == item) {
                return stack;
            }
        }

        return null;
    }

    /* Returns the first empty ItemStack in the inventory, or null if there are none. */
    public ItemStack GetFirstEmptySlot() {
        return GetItemStack(null);
    }

    /* Sets the item and count at slot 'slotIndex'. */
    public void SetItemStack(int slotIndex, Item item, int count) {
        ItemStack stack = GetItemStack(slotIndex);
        if (stack == null) return;

        stack.SetStack(item, count, allowStockpile);
    }

    public void SetItemStack(int slotIndex, ItemStack stack) {
        if (slotIndex >= numSlots) {
            Debug.LogError("Tried to set item at slot " + slotIndex + " when the max slot index is " + (numSlots - 1));
            return;
        }

        stacks[slotIndex] = new ItemStack(stack);

        if (allowStockpile) {
            ConsolidateStacks();
        }
    }

    /* Removes the item at slot 'slotIndex'. */
    public void RemoveItem(int slotIndex) {
        SetItemStack(slotIndex, null, 0);
    }

    /* Add additional slots to this inventory. */
    public void ExpandInventorySize(int additionalSlots) {
        numSlots += additionalSlots;
        while (stacks.Count < numSlots) {
            stacks.Add(new ItemStack(allowStockpile));
        }
    }

    public int GetTotalItemCount(Item item) {
        int count = 0;
        foreach (ItemStack stack in stacks) {
            if (stack.item == item) {
                count += stack.count;
            }
        }
        return count;
    }

    public int TryRemoveItem(Item item, int count) {
        if (count <= 0) return -1;
        if (item == null) return -1;

        ItemStack stack = GetItemStack(item);
        if (stack != null) {
            if (stack.TrySubtractItem(count)) {
                int index = stacks.IndexOf(stack);
                if (stack.count <= 0) {
                    if (allowStockpile) {
                        stacks.Remove(stack);
                        stacks.Add(new ItemStack(allowStockpile));
                    } else {
                        stack.item = null;
                    }
                }
                UpdateUI();
                return index;
            }
        }

        return -1;
    }

    public void ConsolidateStacks() {
        List<int> emptyStacks = new List<int>();
        for (int i = 0; i < stacks.Count; i++) {
            if (stacks[i].item == null) {
                emptyStacks.Add(i);
            } else {
                if (emptyStacks.Count > 0) {
                    ItemStack s = stacks[emptyStacks[0]];
                    stacks[emptyStacks[0]] = stacks[i];
                    stacks[i] = s;
                    emptyStacks.RemoveAt(0);
                    emptyStacks.Add(i);
                }
            }
        }
    }

    public void SavePlayerInventory() {
        savedPlayerInventory = stacks;
    }

    public void DeletePlayerInventory()
    {
        for (int i = 0; i < stacks.Count; i++) {
            RemoveItem(i);
        }
        savedPlayerInventory = stacks;
    }

    public void LoadPlayerInventory() {
        if (savedPlayerInventory == null) return;
        stacks = savedPlayerInventory;
    }

    public void SaveBaseInventory() {
        savedBaseInventory = stacks;
    }

    public void LoadBaseInventory() {
        if (savedBaseInventory == null) return;
        stacks = savedBaseInventory;
    }
    #endregion
}
