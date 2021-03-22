using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public GameObject floorPrefab;

    [HideInInspector]
    public List<GameObject> floorObjects;

    [HideInInspector]
    public GameObject currentFloor;

    public void Initialize() {
        floorObjects = new List<GameObject>();
        currentFloor = Instantiate(floorPrefab, transform);
        floorObjects.Add(currentFloor);
    }

    public void GenerateNextFloor() {
        currentFloor.SetActive(false);
        currentFloor = Instantiate(floorPrefab, transform);
        floorObjects.Add(currentFloor);
    }
}
