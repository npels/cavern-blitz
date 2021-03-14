using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // An enum of all possible items including ores, weapons, and armour 
    public enum Items { rock, ironOre }

    public GameObject rockPrefab;
    public GameObject ironPrefab;


    public GameObject GetItemIcon(Items item)
    {
        if (item == Items.rock)
        {
            return rockPrefab;
        }
        else
        {
            return ironPrefab;
        }
    }


    //mainly for debugging 
    public string GetItemName(Items item)
    {
        if (item == Items.rock)
        {
            return "Rock";
        }
        else
        {
            return "Iron";
        }
    }
}
