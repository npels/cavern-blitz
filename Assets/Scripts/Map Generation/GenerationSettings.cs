﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Generation Assets/GenerationSettings")]
public class GenerationSettings : ScriptableObject {

    public int seed;

    public int caveRadius;
    public int spawnBoxSize;

    [Range(0, 1)]
    public float threshold;
    public float roughness;
    public float falloff;

    public TileBase floorTile;
    public TileBase wallTile;

    [Range(0, 1)]
    public float rockSpawnRate;
    public TileBase rockTile;

    public List<OreSpawnInformation> oreSpawnInformation;

    [System.Serializable]
    public class OreSpawnInformation {
        public float rarity;
        public TileBase oreTile;
    }
}
