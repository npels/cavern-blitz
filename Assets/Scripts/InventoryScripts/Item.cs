using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // An enum of all possible items including ores, weapons, and armour 
    public enum Items { rock, ironOre, empty }

    private Sprite emptySprite;
    private Sprite rockSprite;
    private Sprite ironSprite;

    public GameObject emptyPrefab;
    public GameObject rockPrefab;
    public GameObject ironPrefab;

    private void Awake()
    {
        emptySprite = emptyPrefab.GetComponent<SpriteRenderer>().sprite;
        rockSprite = rockPrefab.GetComponent<SpriteRenderer>().sprite;
        ironSprite = ironPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public Sprite GetItemSprite(Items item)
    {
        if (item == Items.empty)
        {
            return emptySprite;
        }
        else if (item == Items.ironOre)
        {
            return ironSprite;
        } else
        {
            return rockSprite;
        }
    }

}
