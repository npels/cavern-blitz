using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour {

    bool growing = true;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearCrafting = true;
            foreach (Transform child in transform) {
                if (child.GetComponent<SpriteRenderer>()) child.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            TutorialManager.instance.EnableCraftingTip();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            BaseManager.instance.baseUIManager.nearCrafting = false;
            foreach (Transform child in transform) {
                if (child.GetComponent<SpriteRenderer>()) child.GetComponent<SpriteRenderer>().color = Color.white;
            }
            TutorialManager.instance.DisableCraftingTip();
        }
    }

    private void FixedUpdate() {
        foreach (Transform child in transform) {
            if (!child.GetComponent<SpriteRenderer>()) continue;
            if (growing) {
                if (child.localScale.x > 1.05) {
                    growing = false;
                    child.localScale -= Vector3.one * 0.005f;
                } else {
                    child.localScale += Vector3.one * 0.005f;
                }
            } else {
                if (child.localScale.x < 0.95) {
                    growing = true;
                    child.localScale += Vector3.one * 0.005f;
                } else {
                    child.localScale -= Vector3.one * 0.005f;
                }
            }
        }
    }
}
