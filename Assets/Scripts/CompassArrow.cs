using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassArrow : MonoBehaviour {

    public float followDistance;
    public float maxDisance;

    public void FixedUpdate() {
        Vector3 direction = GameManager.instance.mapManager.currentFloor.GetComponent<CaveMap>().staircaseLocation + new Vector3(0.5f, 0.5f, 0) - transform.parent.position;
        if (direction.magnitude < followDistance || direction.magnitude > maxDisance) {
            GetComponent<SpriteRenderer>().enabled = false;
            return;
        } else {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        transform.localPosition = direction.normalized * followDistance;
        float angle = -Vector3.SignedAngle(Vector3.up, direction, Vector3.back);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
