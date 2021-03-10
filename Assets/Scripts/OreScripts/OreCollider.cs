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
        player.SetCanMine(true, this.gameObject);
        print("You are close enough to " + this.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.SetCanMine(false, this.gameObject);
    }

}
