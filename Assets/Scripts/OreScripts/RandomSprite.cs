using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {
    public List<Sprite> sprites;

    private void Start() {
        if (sprites.Count > 0) GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        if (Random.Range(0f, 1f) > 0.5f) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
