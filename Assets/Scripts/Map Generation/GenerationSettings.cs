using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Generation Assets/GenerationSettings")]
public class GenerationSettings : ScriptableObject {

    [Tooltip("The RNG seed for this map.")]
    public int seed;
    [Range(0, 120)]
    [Tooltip("How many tiles are generated in each direction.")]
    public int mapSize;
    [Tooltip("Rough limit for the radius of the cave in tiles.")]
    [Range(0, 100)]
    public int caveRadius;
    [Tooltip("How many tiles around (0, 0, 0) are manually set to be floor tiles.")]
    public int spawnBoxSize;
    [Tooltip("The minimum distance from spawn that a staircase can be placed.")]
    public int minStaircaseDistance;
    [Tooltip("Whether there are any unreachable areas on this map.")]
    public bool removeUnreachableAreas;

    [Range(0, 1)]
    [Tooltip("Any points with a noise value below the threshold are walls, any above the treshold are floors.")]
    public float threshold;
    [Tooltip("The settings for layered noise filters for map generation.")]
    public List<NoiseFilterSettings> noiseFilterSettings;

    [Tooltip("The floor tile to use in generation.")]
    public TileBase floorTile;
    [Tooltip("The wall tile to use in generation.")]
    public TileBase wallTile;

    [Range(0, 1)]
    [Tooltip("The rate at which rocks are placed on floor tiles, scaled by the floor's distance from wall tiles.")]
    public float rockSpawnRate;
    [Tooltip("The base rock tile to use in generation.")]
    public TileBase rockTile;
    [Tooltip("The descending staircase tile to use in generation.")]
    public TileBase staircaseTile;
    [Tooltip("The ascending staircase tile to use in generation.")]
    public TileBase staircaseUpTile;

    [Tooltip("Data for spawning different types of ores.")]
    public List<OreSpawnInformation> oreSpawnInformation;

    [Tooltip("The min/max number of enemies that can spawn on this floor.")]
    public Vector2Int enemySpawnLimits;
    [Tooltip("Data for spawning enemies.")]
    public List<EnemySpawnInformation> enemySpawnInformation;

    [System.Serializable]
    public class NoiseFilterSettings {
        [Tooltip("The 'roughness' of the generated noise map.")]
        public float roughness = 1;
        [Tooltip("The 'strength' which the noise map fluctates between high and low values.")]
        public float strength = 1;
        [Tooltip("How quickly noise values are scaled down as they approach the cave radius.")]
        public float falloff = 1;
    }

    [System.Serializable]
    public class OreSpawnInformation {
        [Tooltip("How likely a rock is to be an ore of this type.")]
        public float rarity;
        [Tooltip("The tile for this ore.")]
        public TileBase oreTile;
    }

    [System.Serializable]
    public class EnemySpawnInformation {
        [Tooltip("How likely this enemy is to spawn.")]
        public float rarity;
        [Tooltip("The prefab for this enemy.")]
        public GameObject enemyPrefab;
    }
}
