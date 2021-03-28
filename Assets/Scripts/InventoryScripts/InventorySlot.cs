using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public ItemStack stack;
    public GameObject itemObject;

    public void UpdateSlot(ItemStack newStack) {
        if (stack == null) {
            stack = newStack;
            if (stack.item != null) {
                itemObject = Instantiate(stack.item.gameObject, transform);
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            }
        } else if (newStack.item == null && stack.item != null) {
            stack = newStack;
            Destroy(itemObject);
            itemObject = null;
            return;
        } else if (newStack.item != stack.item) {
            stack = newStack;
            Destroy(itemObject);
            itemObject = Instantiate(stack.item.gameObject, transform);
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            return;
        }
        stack = newStack;

        if (stack.item != null && itemObject == null) {
            itemObject = Instantiate(stack.item.gameObject, transform);
        }

        if (itemObject != null) {
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            itemObject.transform.localPosition = Vector3.zero;
        }
    }
}
