using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : Ore
{

    void Awake()
    {
        maxHealth = 2;
        currHealth = maxHealth;
        oreName = "Iron Ore";
    }
}
