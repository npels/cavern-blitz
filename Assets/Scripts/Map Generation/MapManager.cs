using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [Tooltip("The prefab for a floor object.")]
    public GameObject floorPrefab;

    [HideInInspector]
    public List<GameObject> floorObjects;

    [HideInInspector]
    public GameObject currentFloor;

    public void Initialize() {
        floorObjects = new List<GameObject>();
        currentFloor = Instantiate(floorPrefab, transform);
        currentFloor.GetComponent<CaveMap>().GenerateCave();
        floorObjects.Add(currentFloor);
    }

    public void GenerateNextFloor() {
        currentFloor.SetActive(false);
        currentFloor = Instantiate(floorPrefab, transform);
        floorObjects.Add(currentFloor);
    }
}
