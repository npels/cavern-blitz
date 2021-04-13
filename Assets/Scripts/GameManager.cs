using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject playerPrefab;

    public MapManager mapManager;

    public UIManager uiManager;

    public Cinemachine.CinemachineVirtualCamera vcam;

    public Inventory inventory;

    [HideInInspector]
    public GameObject player;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        mapManager.Initialize();
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        vcam.Follow = player.transform;
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
        StartCoroutine(uiManager.FadeOut(FinishHomeTransition));
    }

    private void FinishHomeTransition() {
        SceneManager.LoadScene("HomeScene");
    }

    public void PlayerDie() {
        StartCoroutine(uiManager.FadeOut(FinishPlayerDeath));
    }

    private void FinishPlayerDeath() {
        SceneManager.LoadScene("GameOver");
    }
}
