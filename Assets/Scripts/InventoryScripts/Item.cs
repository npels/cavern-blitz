using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    // An enum of all possible items including ores, weapons, and armour 
    public enum Items { rock, ironOre }

    public static Image rockImage;
    public static Image ironImage;

    public static Image GetItemIcon(Items item)
    {
        if (item == Items.rock)
        {
            return rockImage;
        }
        else
        {
            return ironImage;
        }
    }


    //mainly for debugging 
    public static string GetItemName(Items item)
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
