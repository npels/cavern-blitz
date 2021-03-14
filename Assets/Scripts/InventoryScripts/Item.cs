using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // An enum of all possible items including ores, weapons, and armour 
    public enum Items { rock, ironOre }

    private Sprite rockSprite;
    private Sprite ironSprite;

    public GameObject rockPrefab;
    public GameObject ironPrefab;

    private void Start()
    {
        rockSprite = rockPrefab.GetComponent<SpriteRenderer>().sprite;
        ironSprite = ironPrefab.GetComponent<SpriteRenderer>().sprite;

    }

    public Sprite GetItemObject(Items item)
    {
        if (item == Items.rock)
        {
            return rockSprite;
        }
        else
        {
            return ironSprite;
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
