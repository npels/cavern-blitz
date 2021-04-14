using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipes : MonoBehaviour {

    public GameObject recipeSlotPrefab;
    public int numColumns;
    public List<RecipeInfo> recipes;

    private List<RecipeSlot> recipeSlots;

    private void OnEnable() {
        recipeSlots = new List<RecipeSlot>();

        foreach (RecipeSlot slot in GetComponentsInChildren<RecipeSlot>()) {
            recipeSlots.Add(slot);
        }

        for (int i = 0; i < recipes.Count; i++) {
            if (i >= recipeSlots.Count) {
                for (int j = 0; j < 6; j++) {
                    GameObject newSlot = Instantiate(recipeSlotPrefab, recipeSlots[0].transform.parent);
                    recipeSlots.Add(newSlot.GetComponent<RecipeSlot>());
                }
            }

            recipeSlots[i].recipeInfo = recipes[i];
            recipeSlots[i].UpdateSlot();
        }
    }

    public void OpenCrafting() {
        transform.parent.gameObject.SetActive(true);
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = false;
    }

    public void CloseCrafting() {
        transform.parent.gameObject.SetActive(false);
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = true;
    }

    public void UpdateRecipes() {
        foreach (RecipeSlot slot in recipeSlots) {
            slot.UpdateCraftability();
        }
    }

    #region Subclasses
    [System.Serializable]
    public class RecipeItemInfo {
        public Item item;
        public int count;
    }

    [System.Serializable]
    public class RecipeInfo {
        public RecipeItemInfo resultItem;
        public List<RecipeItemInfo> ingredientItems;
    }
    #endregion
}
