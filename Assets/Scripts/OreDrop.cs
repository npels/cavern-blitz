﻿using System.Collections;
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.pickUpSound.Play();
            Debug.Log("pick up ore");
            PickupOre();
        }
    }

    private void FixedUpdate() {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;
        direction.y -= 0.2f;
        if (direction.magnitude < 2) {
            GetComponent<Rigidbody2D>().AddForce(direction.normalized * 10, ForceMode2D.Force);
        }
    }
}
