using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearCrafting = true;
            foreach (Transform child in transform) {
                child.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearCrafting = false;
            foreach (Transform child in transform) {
                child.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
