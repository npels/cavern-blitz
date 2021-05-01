using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : InventorySlot {

    public EquipmentItem.EquipmentType slotType;

    [HideInInspector]
    public EquipmentItem item;

    public override void UpdateSlot(ItemStack newStack) {
        if (stack == null) {
            stack = newStack;
            if (stack.item != null) {
                itemObject = Instantiate(stack.item.gameObject, transform);
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = stack.count != 1;
            }
        } else if (newStack.item == null && stack.item != null) {
            stack = newStack;
            Destroy(itemObject);
            itemObject = null;
            return;
        } else if (newStack.item != stack.item) {
            if (newStack.item != null && !(newStack.item is EquipmentItem)) {
                Debug.LogError("Tried to equip non-equipment item");
                return;
            }
            stack = newStack;
            if (item != null) {
                item.RemoveItem();
            }
            item = (EquipmentItem)newStack.item;
            if (item != null) {
                item.EquipItem();
            }
            Destroy(itemObject);
            itemObject = Instantiate(stack.item.gameObject, transform);
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = stack.count != 1;
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localScale = Vector3.one * scale;
            return;
        }

        if (newStack.item != null && !(newStack.item is EquipmentItem)) {
            Debug.LogError("Tried to equip non-equipment item");
            return;
        }

        if (stack.item != newStack.item) {
            stack = newStack;
            if (item != null) {
                item.RemoveItem();
            }
            item = (EquipmentItem)newStack.item;
            if (item != null) {
                item.EquipItem();
            }
        }

        if (stack.item != null && itemObject == null) {
            itemObject = Instantiate(stack.item.gameObject, transform);
        }

        if (itemObject != null) {
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = stack.count != 1;
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localScale = Vector3.one * scale;
        }
    }

    public void DrawItem() {
        if (itemObject != null) {
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = stack.count != 1;
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localScale = Vector3.one * scale;
        }
    }
}
