using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : Ore
{

    void Awake()
    {
        miningTime = 2;
        oreName = "Iron Ore";
    }
}
