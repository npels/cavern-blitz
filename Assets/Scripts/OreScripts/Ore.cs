using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    //The number of hits it takes to break the ore 
    public int maxHealth;
    //The number of ores that drops when broken
    public int numDrops;
    private int currHealth;

    private void Start()
    {
        currHealth = maxHealth;
    }

    //Decrements the current health of the ore
    public void TakeDamage(int val)
    {
        currHealth -= val;
        Debug.Log(this.tag + " Health: " + currHealth);
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetNumDrops()
    {
        return numDrops;
    }
    
}

