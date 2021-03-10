using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollider : MonoBehaviour
{
    private PlayerMovement player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D col )
    {
        player.canMine = true;
        print("You are close enough to " + col.gameObject.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.canMine = false;
    }


}
