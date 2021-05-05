using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    public static bool completedCraftingTutorial = false;
    public static bool completedStockpileTutorial = false;

    public bool inTutorial = false;

    public GameObject craftingTip;
    public GameObject craftingTutorial;
    public GameObject craftingArrow;
    public GameObject stockpileTip;
    public GameObject stockpileTutorial;
    public GameObject stockpileArrow;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        if (completedStockpileTutorial) {
            stockpileArrow.SetActive(false);
        }
        if (completedCraftingTutorial) {
            craftingArrow.SetActive(false);
        }
    }

    public void EnableCraftingTip() {
        if (completedCraftingTutorial) return;
        craftingTip.SetActive(true);
    }

    public void DisableCraftingTip() {
        craftingTip.SetActive(false);
    }

    public void EnableStockpileTip() {
        if (completedStockpileTutorial) return;
        stockpileTip.SetActive(true);
    }

    public void DisableStockpileTip() {
        stockpileTip.SetActive(false);
    }

    public void StartCraftingTutorial() {
        if (completedCraftingTutorial) return;
        craftingTutorial.SetActive(true);
        craftingArrow.SetActive(false);
        inTutorial = true;
        completedCraftingTutorial = true;
    }

    public void StartStockpileTutorial() {
        if (completedStockpileTutorial) return;
        stockpileTutorial.SetActive(true);
        stockpileArrow.SetActive(false);
        inTutorial = true;
        completedStockpileTutorial = true;
    }
}
