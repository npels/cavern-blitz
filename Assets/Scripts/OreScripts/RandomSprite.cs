using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {
    public List<Sprite> sprites;

    private void Start() {
        if (sprites.Count > 0) GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        if (Random.Range(0f, 1f) > 0.5f) GetComponentInChildren<SpriteRenderer>().flipX = true;
    }
}
