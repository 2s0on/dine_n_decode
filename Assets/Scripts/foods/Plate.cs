using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private RectTransform rt; // stores rect transform of this plate
    private Vector3 originalLocalScale; // stores initial scale to reset later
    private Transform originalParent; // stores original parent for resetting
    private Vector2 originalAnchoredPosition; // stores initial ui anchored position

    private string pendingModifier; // stores a modifier to apply to the next ingredient
    private List<string> ingredients = new List<string>(); // stores list of ingredients on the plate

    void Awake()
    {
        rt = GetComponent<RectTransform>(); // get rect transform

        // store initial transform data
        originalLocalScale = rt.localScale;
        originalParent = rt.parent;
        originalAnchoredPosition = rt.anchoredPosition;
    }

    public void AddFood(FoodItem item)
    {
        if (item == null) return;

            // NOTE: modifiers must come before ingredients for this to work
        if (item.type == FoodType.Modifier)
        {
            pendingModifier = item.itemName; // store modifier for next ingredient
            Debug.Log($"Pending modifier set: {pendingModifier}");
            return;
        }

        if (item.type == FoodType.Ingredient)
        {
            // combine modifier and ingredient if modifier exists
            string finalName = string.IsNullOrEmpty(pendingModifier)
                ? item.itemName
                : $"{pendingModifier} {item.itemName}";

            ingredients.Add(finalName.Trim()); // add to ingredients list
            Debug.Log($"Added to plate: {finalName}");

            pendingModifier = null; // clear modifier after applying
        }
    }

    public void AddIngredient(string ingredient)
    {
        ingredients.Add(ingredient.ToLower().Trim()); // add ingredient directly, normalized
        Debug.Log($"Added to plate: {ingredient.ToLower().Trim()}");
    }

    public List<string> GetIngredients()
    {
        return ingredients; // return current ingredient list
    }

    public void RemoveIngredient(string ingredientName)
    {
        ingredients.RemoveAll(i => i == ingredientName); // remove all matching ingredients
        Debug.Log($"Removed {ingredientName} from plate!");
    }

    public void ResetPlate()
    {
        ingredients.Clear(); // clear ingredients
        pendingModifier = null; // clear any pending modifier

        // remove all children that are not the plate itself
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (!child.CompareTag("Plate"))
                Destroy(child.gameObject);
        }

        // reset UI and transform data-
        transform.SetParent(originalParent, false);
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = originalAnchoredPosition;
        rt.localScale = originalLocalScale;

        Debug.Log("Plate reset!");
    }
}
