using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour {

    public Transform itemSpriteParent;
    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI itemAttributes;
    public Button craftButton;
    public List<Transform> ingredientSlots;
    public List<GameObject> ingredientRedOverlays;

    [HideInInspector]
    public CraftingRecipes.RecipeInfo recipe = null;

    private GameObject itemSprite;
    private List<GameObject> ingredientObjects;

    private void Start() {
        ingredientObjects = new List<GameObject>();
    }

    private void OnDisable() {
        if (itemSprite != null) {
            Destroy(itemSprite);
        }
        itemSprite = null;

        recipe = null;
        itemName.text = "";
        itemAttributes.text = "";
        craftButton.interactable = false;

        if (ingredientObjects != null) {
            foreach (GameObject ingredient in ingredientObjects) {
                Destroy(ingredient);
            }
            ingredientObjects.Clear();
        }

        foreach (GameObject slot in ingredientRedOverlays) {
            slot.SetActive(false);
        }
    }

    public void UpdateRecipeInfo(CraftingRecipes.RecipeInfo recipe) {
        this.recipe = recipe;

        if (itemSprite != null) {
            Destroy(itemSprite);
        }
        Item item = recipe.resultItem.item;

        itemSprite = Instantiate(item.gameObject, itemSpriteParent);
        itemSprite.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;

        itemName.text = item.itemName;

        string newAttributes = "";
        for (int i = 0; i < item.attributeText.Length; i++) {
            if (item.attributeText[i] == '$') newAttributes += "\n";
            else newAttributes += item.attributeText[i];
        }
        itemAttributes.text = "\"" + item.flavorText + "\"\n" + newAttributes;

        foreach (GameObject ingredient in ingredientObjects) {
            Destroy(ingredient);
        }

        ingredientObjects.Clear();

        bool craftable = true;
        for (int i = 0; i < recipe.ingredientItems.Count; i++) {
            ingredientObjects.Add(Instantiate(recipe.ingredientItems[i].item.gameObject, ingredientSlots[i]));

            ingredientObjects[i].transform.SetSiblingIndex(0);
            ingredientObjects[i].transform.localScale = Vector3.one * 0.9f;
            ingredientObjects[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = recipe.ingredientItems[i].count.ToString();
            ingredientObjects[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = recipe.ingredientItems[i].count != 1;
        }

        for (int i = 0; i < ingredientSlots.Count; i++) {
            bool enoughItems = true;
            if (i < recipe.ingredientItems.Count && BaseManager.instance.stockpileInventory.GetTotalItemCount(recipe.ingredientItems[i].item) < recipe.ingredientItems[i].count) {
                enoughItems = false;
                craftable = false;
            }
            ingredientRedOverlays[i].SetActive(!enoughItems);
        }

        craftButton.interactable = craftable;
    }
}
