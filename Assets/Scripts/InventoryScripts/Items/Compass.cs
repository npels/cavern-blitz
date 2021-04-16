using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Compass : EquipmentItem {

    public GameObject compassArrowPrefab;

    private GameObject compassArrowObject;

    public override void EquipItem() {
        base.EquipItem();
        if (SceneManager.GetActiveScene().name.Equals("GameScene")) {
            compassArrowObject = Instantiate(compassArrowPrefab, GameManager.instance.player.transform);
        }
    }

    public override void RemoveItem() {
        base.RemoveItem();
        if (SceneManager.GetActiveScene().name.Equals("GameScene")) {
            Destroy(compassArrowObject);
        }
    }
}
