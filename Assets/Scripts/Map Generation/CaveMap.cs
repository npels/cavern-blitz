using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveMap : MonoBehaviour {

    public bool autoUpdateInEditor;

    public GenerationSettings settings;

    CaveGenerator caveGenerator;

    Tilemap tilemap;

    List<Vector3Int> floorLocations;
    List<Vector3Int> wallLocations;

    void Initialize() {
        floorLocations = new List<Vector3Int>();
        wallLocations = new List<Vector3Int>();

        tilemap = GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        caveGenerator = new CaveGenerator(settings);
    }

    void GenerateTiles() {
        for (int x = -settings.caveRadius; x < settings.caveRadius; x++) {
            for (int y = -settings.caveRadius; y < settings.caveRadius; y++) {
                bool isWall = caveGenerator.IsWallAtPoint(x, y);

                if (isWall) {
                    wallLocations.Add(new Vector3Int(x, y, 0));
                } else {
                    floorLocations.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        for (int x = -settings.spawnBoxSize; x < settings.spawnBoxSize; x++) {
            for (int y = -settings.spawnBoxSize; y < settings.spawnBoxSize; y++) {
                floorLocations.Add(new Vector3Int(x, y, 0));
            }
        }

        TileBase[] wallTiles = new TileBase[wallLocations.Count];
        for (int i = 0; i < wallLocations.Count; i++) wallTiles[i] = settings.wallRuleTile;

        TileBase[] floorTiles = new TileBase[floorLocations.Count];
        for (int i = 0; i < floorLocations.Count; i++) floorTiles[i] = settings.floorTile;


        tilemap.SetTiles(wallLocations.ToArray(), wallTiles);
        tilemap.SetTiles(floorLocations.ToArray(), floorTiles);
    }

    public void GenerateCave() {
        Initialize();
        GenerateTiles();
    }
}
