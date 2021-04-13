using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {

    public static CraftingManager instance;

    public CraftingRecipes craftingRecipes;
    public Inventory stockpileInventory;
    public ItemInfoUI itemInfoUI;

    private void Awake() {
        instance = this;
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
