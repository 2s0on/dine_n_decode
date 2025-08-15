using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private List<string> ingredients = new List<string>();
    private string pendingModifier = null; // Store modifier until next ingredient

    //prevents plate from scaling when parented to a new object
    private Vector3 originalLocalScale;

    void Awake()
    {
        originalLocalScale = transform.localScale;
    }

    public void SetParentAndKeepScale(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localScale = originalLocalScale;
    }

    // call this with a FoodItem so we can decide if it's a modifier or ingredient
    public void AddFood(FoodItem item)
    {
        if (item == null) return;

        if (item.type == FoodType.Modifier)

        {
            pendingModifier = item.itemName; // store modifier for next ingredient
            Debug.Log($"Pending modifier set: {pendingModifier}");
            return;
        }

        if (item.type == FoodType.Ingredient)
        {
            // Combine with modifier **once** here
            string finalName = string.IsNullOrEmpty(pendingModifier)
                ? item.itemName
                : $"{pendingModifier} {item.itemName}";

            ingredients.Add(finalName);
            Debug.Log($"Added to plate: {finalName}");

            pendingModifier = null; // clear after use
        }
    }

    // NOTE: MODIFIERS MUST COME BEFORE INGREDIENTS FOR THIS TO WORK

    // Remove the modifier logic from AddIngredient entirely:
    public void AddIngredient(string ingredient)
    {
        ingredients.Add(ingredient);
        Debug.Log($"Added to plate: {ingredient}");
    }


    public List<string> GetIngredients()
    {
        return ingredients;

    }
}

