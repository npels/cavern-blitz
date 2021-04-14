using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileChest : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearStockpile = true;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearStockpile = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
