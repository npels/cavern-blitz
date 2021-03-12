using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    //The number of hits it takes to break the ore 
    protected int maxHealth;
    protected int currHealth;

    private void Start()
    {
        maxHealth = 1;
        currHealth = maxHealth;
    }

    public void TakeDamage(int val)
    {
        currHealth -= val;
        Debug.Log(this.tag + " Health: " + currHealth);
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
