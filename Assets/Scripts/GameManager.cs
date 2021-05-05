using System.Collections;
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

    public RopeItem ropeItem;

    public AudioSource pickUpSound;

    public AudioSource enemyDamage;

    public AudioSource enemyDeath;

    public AudioSource stairSound;

    public AudioSource ropeSound;

    private bool usingStairs;

    [HideInInspector]
    public bool setRope = false;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        mapManager.Initialize();
        pickUpSound = GetComponents<AudioSource>()[1];
        enemyDamage = GetComponents<AudioSource>()[2];
        enemyDeath = GetComponents<AudioSource>()[3];
        stairSound = GetComponents<AudioSource>()[4];
        ropeSound = GetComponents<AudioSource>()[5];
        vcam.Follow = player.transform;
        inventory.LoadPlayerInventory();
    }

    public void GotoNextFloor() {
        uiManager.CloseDescendMessage();
        if (setRope) RemoveRope();
        setRope = false;
        if (PlayerAttributes.trinket is BloodAmulet)
        {
            BloodAmulet b = (BloodAmulet)PlayerAttributes.trinket;
            player.GetComponent<PlayerInteractions>().HealPlayer(b.healAmount);
        }
        StartCoroutine(uiManager.FadeOut(FinishFloorTransition, stairSound));

    }


    private void FinishFloorTransition() {
        mapManager.GenerateNextFloor();
        vcam.ForceCameraPosition(new Vector3(0, 0, -10), Quaternion.identity);
        player.transform.position = Vector3.zero;
        StartCoroutine(uiManager.FadeIn());
    }

    public void ReturnToHome() {
        inventory.SavePlayerInventory();
        StartCoroutine(uiManager.FadeOut(FinishHomeTransition, stairSound));

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

    private void RemoveRope() {
        inventory.TryRemoveItem(ropeItem, 1);
    }
}
