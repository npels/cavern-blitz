using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour {
    public bool descending;

    private AudioSource stairAudio;
    private void Start()
    {
        stairAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            
            if (descending) {
                GameManager.instance.uiManager.OpenDescendMessage();
            } else {
                GameManager.instance.uiManager.OpenLeaveMessage();
            }
            
        }
    }
}
