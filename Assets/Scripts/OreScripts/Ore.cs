using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    protected string oreName;
    //The number of hits it takes to break the ore 
    protected int maxHealth;
    protected int currHealth;

    private void Start()
    {
        maxHealth = 1;
        currHealth = maxHealth;
        oreName = "this is a generic ore";
    }

    public void PrintOre()
    {
        print(oreName);
    }
    public void TakeDamage(int val)
    {
        currHealth -= val;
    }
}
