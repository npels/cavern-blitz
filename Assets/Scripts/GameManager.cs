﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MapManager mapManager;

    public UIManager uiManager;

    public Cinemachine.CinemachineVirtualCamera vcam;

    public Inventory inventory;

    public GameObject player;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        mapManager.Initialize();
        vcam.Follow = player.transform;
        inventory.LoadPlayerInventory();
    }

    public void GotoNextFloor() {
        uiManager.CloseDescendMessage();
        StartCoroutine(uiManager.FadeOut(FinishFloorTransition));
    }

    private void FinishFloorTransition() {
        mapManager.GenerateNextFloor();
        vcam.ForceCameraPosition(new Vector3(0, 0, -10), Quaternion.identity);
        player.transform.position = Vector3.zero;
        StartCoroutine(uiManager.FadeIn());
    }

    public void ReturnToHome() {
        inventory.SavePlayerInventory();
        StartCoroutine(uiManager.FadeOut(FinishHomeTransition));
    }

    private void FinishHomeTransition() {
        SceneManager.LoadScene("HomeScene");
    }

    public void PlayerDie() {
        inventory.DeletePlayerInventory();
        StartCoroutine(uiManager.FadeOut(FinishPlayerDeath));
    }

    private void FinishPlayerDeath() {
        SceneManager.LoadScene("GameOver");
    }
}
