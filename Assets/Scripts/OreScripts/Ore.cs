using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    protected string oreName;

    private void Start()
    {
        oreName = "this is a generic ore";
    }

    public void PrintOre()
    {
        print(oreName);
    }
}
