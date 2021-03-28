using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack {

    public Item item;
    public int count;
    public int maxCount;

    private bool allowStockpile;

    public static int MAX_STOCKPILE_SIZE = 999999;

    public ItemStack(bool allowStockpile = false, Item item = null, int count = 0) {
        SetStack(item, count, allowStockpile);
    }

    public ItemStack(ItemStack stack) {
        SetStack(stack.item, stack.count, stack.allowStockpile);
    }

    public void SetStack(Item item, int count, bool allowStockpile) {
        this.item = item;
        this.count = count;
        this.allowStockpile = allowStockpile;
        if (item == null) maxCount = 0;
        else maxCount = allowStockpile ? MAX_STOCKPILE_SIZE : item.maxStack;
    }

    public void AddToStack(int num) {
        count += num;
    }

    public void SetCount(int num) {
        count = num;
    }

    public bool TryStackItem(Item newItem, int num) {
        if (item == null) {
            item = newItem;
            count = num;
            if (item == null) maxCount = 0;
            else maxCount = allowStockpile ? MAX_STOCKPILE_SIZE : item.maxStack;
            return true;
        }
        if (newItem != item) return false;
        if (count + num > maxCount) return false;
        count += num;
        return true;
    }
}
