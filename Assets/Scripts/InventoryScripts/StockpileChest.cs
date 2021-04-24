using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileChest : MonoBehaviour {

    bool growing = false;

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

    private void FixedUpdate() {
        if (growing) {
            if (transform.localScale.x > 1.05) {
                growing = false;
                transform.localScale -= Vector3.one * 0.005f;
            } else {
                transform.localScale += Vector3.one * 0.005f;
            }
        } else {
            if (transform.localScale.x < 0.95) {
                growing = true;
                transform.localScale += Vector3.one * 0.005f;
            } else {
                transform.localScale -= Vector3.one * 0.005f;
            }
        }
    }
}
