using System.Collections;
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

    public Tile wallTile;
    public Tile floorTile;
    public RuleTile wallRuleTile;
}
