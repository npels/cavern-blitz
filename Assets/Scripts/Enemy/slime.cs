using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime : Enemy
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player")) {
            col.transform.GetComponent<PlayerInteractions>().takeDamage(attackDamage);
        }
    }
    protected override void updateAnim() {
        
    }
}
