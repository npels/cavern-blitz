﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [Tooltip("The name of this item.")]
    public string itemName;
    [Tooltip("The maximum number of this item that can fit in a single stack.")]
    public int maxStack;
}
