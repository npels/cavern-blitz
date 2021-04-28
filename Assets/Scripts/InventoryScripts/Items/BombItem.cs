using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : ConsumableItem {

    public GameObject bombPrefab;

    public override int Activate() {
        Instantiate(bombPrefab, GameManager.instance.player.transform.position, Quaternion.identity);
        return -1;
    }
}
