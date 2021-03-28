using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour {
    public int maxHealth; //The number of hits it takes to break the ore 
    public int numDrops; //The number of ores that drops when broken
    private int currHealth;
    public Item oreItem;

    private bool isPickupable = false;

    private void Start()
    {
        currHealth = maxHealth;
    }

    //Decrements the current health of the ore
    public void TakeDamage(int val)
    {
        currHealth -= val;
        if (currHealth <= 0)
        {
            DropOre(this.gameObject);
        }
    }

    public int GetNumDrops()
    {
        return numDrops;
    }

    #region Drop Functions/Animations
    private void DropOre(GameObject g)
    {
        g.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        isPickupable = true;
    }
    private void PickupOre()
    {
        if (oreItem == null) {
            isPickupable = false;
            Destroy(gameObject);
        }
        if (GameManager.instance.inventory.TryAddItem(oreItem, 1) != -1) {
            isPickupable = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isPickupable)
        {
            PickupOre();
        }
    }


    #endregion
}
