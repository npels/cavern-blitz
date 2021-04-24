using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCaveStairs : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        BaseManager.instance.baseUIManager.OpenDescendMessage();
    }
}
