using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour {

    public static BaseManager instance;

    public CraftingRecipes craftingRecipes;
    public Inventory playerInventory;
    public Inventory stockpileInventory;
    public ItemInfoUI itemInfoUI;

    public BaseUIManager baseUIManager;
    public GameObject player;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        stockpileInventory.LoadBaseInventory();
        if (stockpileInventory.stacks.Count == 0) {
            stockpileInventory.InitInventory();
        }
        playerInventory.LoadPlayerInventory();
        StartCoroutine(baseUIManager.FadeIn());
    }

    public void EnterCave() {
        stockpileInventory.SaveBaseInventory();
        playerInventory.SavePlayerInventory();
        StartCoroutine(baseUIManager.FadeOut(FinishEnterCave));
    }

    public void FinishEnterCave() {
        SceneManager.LoadScene("GameScene");
    }

    public void CraftItem() {
        CraftingRecipes.RecipeInfo recipe = itemInfoUI.recipe;
        if (recipe == null) {
            Debug.LogError("Tried to craft null recipe");
            return;
        }

        foreach (CraftingRecipes.RecipeItemInfo ingredient in recipe.ingredientItems) {
            int slot = stockpileInventory.TryRemoveItem(ingredient.item, ingredient.count);
            if (slot == -1) {
                Debug.LogError("Tried to craft item with not enough resources");
                return;
            }
        }

        int slot2 = stockpileInventory.TryAddItem(recipe.resultItem.item, recipe.resultItem.count);
        if (slot2 == -1) {
            Debug.LogError("Failed to add crafted item to inventory");
            return;
        }

        craftingRecipes.UpdateRecipes();
    }
}
