using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    protected string oreName;
    protected int miningTime;

    private void Start()
    {
        miningTime = 0;
        oreName = "this is a generic ore";
    }

    public void PrintOre()
    {
        print(oreName);
    }
}
