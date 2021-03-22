﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveMap : MonoBehaviour {

    public bool autoUpdateInEditor;

    public GenerationSettings settings;

    CaveGenerator caveGenerator;

    Tilemap tilemap;
    Tilemap oreTilemap;

    float[] valueMap;
    
    List<Vector3Int> floorLocations;
    List<Vector3Int> wallLocations;
    List<Vector3Int> rockLocations;

    private void Start() {
        GenerateCave();
    }

    void Initialize() {
        valueMap = new float[settings.mapSize * settings.mapSize * 4];

        floorLocations = new List<Vector3Int>();
        wallLocations = new List<Vector3Int>();
        rockLocations = new List<Vector3Int>();

        tilemap = GetComponent<Tilemap>();
        tilemap.ClearAllTiles();

        oreTilemap = transform.GetChild(0).GetComponent<Tilemap>();
        oreTilemap.ClearAllTiles();

        caveGenerator = new CaveGenerator(settings);
        Random.InitState(settings.seed);
    }

    void GenerateMap() {
        PopulateValueMap();
        FilterValueMap();
        DecideTiles();
        ClearSpawnArea();
        SetTiles();
    }

    public void GenerateCave() {
        Initialize();
        GenerateMap();
    }

    void PopulateValueMap() {
        for (int x = -settings.mapSize; x < settings.mapSize; x++) {
            for (int y = -settings.mapSize; y < settings.mapSize; y++) {
                SetPointValue(x, y, caveGenerator.IsWallAtPoint(x, y));
            }
        }
    }

    void FilterValueMap() {
        bool alteredMap = false;
        int numIterations = 0;
        while (numIterations < 1000) {
            for (int x = -settings.mapSize; x < settings.mapSize; x++) {
                for (int y = -settings.mapSize; y < settings.mapSize; y++) {
                    bool valid = CheckValueValid(x, y);
                    if (!valid) {
                        SetPointValue(x, y, settings.threshold);
                        alteredMap = true;
                    }
                }
            }
            if (!alteredMap) break;
            alteredMap = false;
            numIterations++;
        }
        Debug.Log("Filtered map with " + numIterations + " iterations");
    }

    void DecideTiles() {
        for (int x = -settings.mapSize; x < settings.mapSize; x++) {
            for (int y = -settings.mapSize; y < settings.mapSize; y++) {
                float value = GetPointValue(x, y);
                if (value < settings.threshold) {
                    wallLocations.Add(new Vector3Int(x, y, 0));
                } else {
                    floorLocations.Add(new Vector3Int(x, y, 0));

                    float rockSpawnValue = value * Random.Range(0.5f, 2f);

                    if (rockSpawnValue < settings.rockSpawnRate) {
                        rockLocations.Add(new Vector3Int(x, y, 0));
                    }
                }
            }
        }
    }

    void ClearSpawnArea() {
        for (int x = -settings.spawnBoxSize; x < settings.spawnBoxSize; x++) {
            for (int y = -settings.spawnBoxSize; y < settings.spawnBoxSize; y++) {
                floorLocations.Add(new Vector3Int(x, y, 0));
            }
        }
    }

    void SetTiles() {
        TileBase[] wallTiles = new TileBase[wallLocations.Count];
        for (int i = 0; i < wallLocations.Count; i++) wallTiles[i] = settings.wallTile;

        TileBase[] floorTiles = new TileBase[floorLocations.Count];
        for (int i = 0; i < floorLocations.Count; i++) floorTiles[i] = settings.floorTile;

        TileBase[] oreTiles = new TileBase[rockLocations.Count];
        for (int i = 0; i < rockLocations.Count; i++) oreTiles[i] = settings.rockTile;

        List<GenerationSettings.OreSpawnInformation> oreSpawnInformation = settings.oreSpawnInformation;
        ShuffleList(oreSpawnInformation);

        for (int i = 0; i < rockLocations.Count; i++) {
            float oreSpawnValue = Random.Range(0f, 1f);
            foreach (GenerationSettings.OreSpawnInformation ore in oreSpawnInformation) {
                if (oreSpawnValue < ore.rarity) {
                    oreTiles[i] = ore.oreTile;
                }
            }
        }

        tilemap.SetTiles(wallLocations.ToArray(), wallTiles);
        tilemap.SetTiles(floorLocations.ToArray(), floorTiles);
        oreTilemap.SetTiles(rockLocations.ToArray(), oreTiles);
    }

    bool CheckValueValid(int x, int y) {
        float value = GetPointValue(x, y);
        if (value < settings.threshold) {
            if (GetPointValue(x - 1, y - 1) >= settings.threshold && GetPointValue(x + 1, y + 1) >= settings.threshold) {
                return false;
            }
            if (GetPointValue(x, y - 1) >= settings.threshold && GetPointValue(x, y + 1) >= settings.threshold) {
                return false;
            }
            if (GetPointValue(x + 1, y - 1) >= settings.threshold && GetPointValue(x - 1, y + 1) >= settings.threshold) {
                return false;
            }
            if (GetPointValue(x - 1, y) >= settings.threshold && GetPointValue(x + 1, y) >= settings.threshold) {
                return false;
            }
        }
        return true;
    }

    void ShuffleList<T>(List<T> list) {
        int count = list.Count;
        for (int i = 0; i < count; i++) {
            int r = i + Random.Range(0, count - i);
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }

    public int ValueMapIndex(int x, int y) {
        return (x + settings.mapSize) + (y + settings.mapSize) * settings.mapSize * 2;
    }

    public float GetPointValue(int x, int y) {
        if (x < -settings.mapSize || x >= settings.mapSize || y < -settings.mapSize || y >= settings.mapSize) return 0;
        return valueMap[ValueMapIndex(x, y)];
    }

    public void SetPointValue(int x, int y, float value) {
        if (x < -settings.mapSize || x >= settings.mapSize || y < -settings.mapSize || y >= settings.mapSize) return;
        valueMap[ValueMapIndex(x, y)] = value;
    }
}
