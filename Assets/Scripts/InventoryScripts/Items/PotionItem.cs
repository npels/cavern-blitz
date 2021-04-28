using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : ConsumableItem {

    public float healAmount;

    public override int Activate() {
        GameManager.instance.player.GetComponent<PlayerInteractions>().HealPlayer(healAmount);
        return -1;
    }
}
