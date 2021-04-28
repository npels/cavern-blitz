using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeItem : ConsumableItem {

    public override int Activate() {
        if ((GameManager.instance.player.transform.position - Vector3.one * 0.5f).magnitude > 3) {
            if (GameManager.instance.setRope) {
                GameManager.instance.setRope = false;
                GameManager.instance.player.transform.position = new Vector3(0.5f, 0.5f, 0);
                return -1;
            } else {
                GameManager.instance.uiManager.OpenRopeTip();
                return 0;
            }
        } else if (!GameManager.instance.setRope) {
            GameManager.instance.uiManager.SetRopeMessage();
            GameManager.instance.setRope = true;
            return 0;
        } else {
            return 0;
        }
    }
}
