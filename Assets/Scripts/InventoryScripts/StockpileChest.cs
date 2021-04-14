using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileChest : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearStockpile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearStockpile = false;
        }
    }
}
