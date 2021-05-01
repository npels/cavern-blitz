using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationRune : ConsumableItem {

    public float delay;

    public override int Activate() {
        if (GameManager.instance.player.GetComponent<PlayerMovement>().canMove) {
            GameManager.instance.player.GetComponent<PlayerMovement>().Teleport(delay);
            return -1;
        }
        return 0;
    }

    
}
