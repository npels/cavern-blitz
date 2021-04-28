using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDrop : MonoBehaviour
{
    public Item oreItem;

    private void PickupOre()
    {
        if (oreItem == null)
        {
            Destroy(gameObject);
        }
        if (GameManager.instance.inventory.TryAddItem(oreItem, 1) != -1)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("pick up ore");
            PickupOre();
        }
    }
}
