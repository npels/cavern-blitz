using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour {
    private AudioSource stairAudio;
    private void Start()
    {
        stairAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            stairAudio.Play();
            GameManager.instance.GotoNextFloor();
        }
    }
}
