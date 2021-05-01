using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationRune : ConsumableItem {

    public float delay;

    public override int Activate() {
        GameManager.instance.player.GetComponent<PlayerMovement>().Teleport(delay);
        return -1;
    }

    
}
