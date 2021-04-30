using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [Tooltip("The prefab for a floor object.")]
    public List<GameObject> floorPrefabs;

    [HideInInspector]
    public List<GameObject> floorObjects;

    [HideInInspector]
    public GameObject currentFloor;

    [HideInInspector]
    public int floorNumber = 0;

    [HideInInspector]
    public int prefabNumber = 0;

    [HideInInspector]
    public GameObject currentPrefab;

    public void Initialize() {
        floorObjects = new List<GameObject>();
        currentPrefab = floorPrefabs[prefabNumber];
        currentFloor = Instantiate(currentPrefab, transform);
        currentFloor.GetComponent<CaveMap>().GenerateRandomCave(++floorNumber); ;
        floorObjects.Add(currentFloor);
    }

    public void GenerateNextFloor() {
        currentFloor.SetActive(false);
        if (currentFloor.GetComponent<CaveMap>().settings.endFloors.y < floorNumber && prefabNumber < floorPrefabs.Count - 1) {
            currentPrefab = floorPrefabs[++prefabNumber];
        }
        currentFloor = Instantiate(currentPrefab, transform);
        currentFloor.GetComponent<CaveMap>().GenerateRandomCave(++floorNumber);
        floorObjects.Add(currentFloor);
    }
}
