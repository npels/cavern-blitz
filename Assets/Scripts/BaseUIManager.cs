using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIManager : MonoBehaviour {

    public float fadeSpeed = 1;
    public Image blackout;
    public GameObject descendMessage;
    public GameObject tutorialDescendMessage;
    public InventoryUI playerInventoryUI;
    public CraftingRecipes craftingUI;
    public StockpileTransferUI stockpileUI;

    

    public delegate void OnFadeFunction();

    [HideInInspector]
    public bool nearCrafting = false;
    [HideInInspector]
    public bool nearStockpile = false;
    [HideInInspector]
    public AudioSource openChestSound;
    [HideInInspector]
    public AudioSource closeChestSound;
    private bool inventoryOpen = false;

    [HideInInspector]
    public bool fading = false;

    private void Start()
    {
        openChestSound = GetComponents<AudioSource>()[0];
        closeChestSound = GetComponents<AudioSource>()[1];
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (inventoryOpen) {
                if (nearCrafting) {
                    craftingUI.CloseCrafting();
                } else if (nearStockpile) {
                    closeChestSound.Play();
                    stockpileUI.CloseInventory();
                } else {
                    playerInventoryUI.CloseInventory();
                }
                inventoryOpen = false;
            } else {
                if (nearCrafting) {
                    craftingUI.OpenCrafting();
                    TutorialManager.instance.DisableCraftingTip();
                    TutorialManager.instance.StartCraftingTutorial();
                } else if (nearStockpile) {
                    openChestSound.Play();
                    stockpileUI.OpenInventory();
                    TutorialManager.instance.DisableStockpileTip();
                    TutorialManager.instance.StartStockpileTutorial();
                } else {
                    playerInventoryUI.OpenInventory();
                }
                inventoryOpen = true;
            }
        }
    }

    public void OpenDescendMessage() {
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = false;
        if (TutorialManager.completedCraftingTutorial && TutorialManager.completedStockpileTutorial) descendMessage.SetActive(true);
        else tutorialDescendMessage.SetActive(true);
    }

    public void CloseDescendMessage() {
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = true;
        if (TutorialManager.completedCraftingTutorial && TutorialManager.completedStockpileTutorial) descendMessage.SetActive(false);
        else tutorialDescendMessage.SetActive(false);
    }

    public IEnumerator FadeOut(OnFadeFunction func = null) {
        while (fading) yield return null;

        fading = true;
        while (blackout.color.a < 0.95f) {
            Color c = blackout.color;
            c = Color.Lerp(c, Color.black, Time.deltaTime * fadeSpeed);
            blackout.color = c;
            yield return null;
        }

        blackout.color = Color.black;
        yield return null;

        fading = false;

        func?.Invoke();
    }

    public IEnumerator FadeIn(OnFadeFunction func = null) {
        while (fading) yield return null;

        fading = true;
        while (blackout.color.a > 0.05f) {
            Color c = blackout.color;
            c = Color.Lerp(c, Color.clear, Time.deltaTime * fadeSpeed);
            blackout.color = c;
            yield return null;
        }

        blackout.color = Color.clear;
        fading = false;

        func?.Invoke();
    }
}
