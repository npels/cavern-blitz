using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public float scale;

    [HideInInspector]
    public ItemStack stack;
    [HideInInspector]
    public GameObject itemObject;

    public virtual void UpdateSlot(ItemStack newStack) {
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
            stack = newStack;
            Destroy(itemObject);
            itemObject = Instantiate(stack.item.gameObject, transform);
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = stack.count.ToString();
            itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = stack.count != 1;
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localScale = Vector3.one * scale;
            return;
        }
        stack = newStack;

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
}
