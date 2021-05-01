using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionRune : ConsumableItem {

    public float duration;

    public override int Activate() {
        if (!GameManager.instance.player.GetComponent<PlayerInteractions>().invulnerable) {
            GameManager.instance.player.GetComponent<PlayerInteractions>().Protection(duration);
            return -1;
        }
        return 0;
    }
}
