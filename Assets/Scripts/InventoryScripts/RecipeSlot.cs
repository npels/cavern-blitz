using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSlot : MonoBehaviour {

    public CraftingRecipes.RecipeInfo recipeInfo;
    public GameObject itemObject;

    public void UpdateSlot() {
        if (itemObject != null) {
            Destroy(itemObject);
        }

        itemObject = Instantiate(recipeInfo.resultItem.item.gameObject, transform);
        itemObject.transform.SetSiblingIndex(itemObject.transform.GetSiblingIndex() - 1);
        itemObject.GetComponent<RectTransform>().localScale = Vector3.one * 0.9f;
        if (itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>()) {
            if (recipeInfo.resultItem.count == 1) {
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
            } else {
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = true;
                itemObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = recipeInfo.resultItem.count.ToString();
            }
        }

        UpdateCraftability();
    }

    public void UpdateCraftability() {
        bool craftable = true;
        foreach (CraftingRecipes.RecipeItemInfo ingredient in recipeInfo.ingredientItems) {
            if (CraftingManager.instance.stockpileInventory.GetTotalItemCount(ingredient.item) < ingredient.count) {
                craftable = false;
                break;
            }
        }

        transform.GetChild(transform.childCount - 1).gameObject.SetActive(!craftable);
    }

    public void SelectRecipe() {
        if (recipeInfo.resultItem.item == null) return;
        CraftingManager.instance.itemInfoUI.UpdateRecipeInfo(recipeInfo);
    }
}
