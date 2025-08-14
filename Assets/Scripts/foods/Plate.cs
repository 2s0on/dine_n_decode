using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private List<string> ingredients = new List<string>();
    private string pendingModifier = null; // Store modifier until next ingredient

    public void AddModifier(string modifier)
    {
        // Example: "Spicy", "Double", "Sweet"
        pendingModifier = pendingModifier == null ? modifier : pendingModifier + " " + modifier;
        Debug.Log($"Pending modifier: {pendingModifier}");
    }

    public void AddIngredient(string ingredient)
    {
        string finalName = ingredient;

        // If there's a pending modifier, combine it with this ingredient
        if (!string.IsNullOrEmpty(pendingModifier))
        {
            finalName = pendingModifier + " " + ingredient;
            pendingModifier = null; // Clear after using
        }

        ingredients.Add(finalName);
        Debug.Log($"Added to plate: {finalName}");
    }

    public List<string> GetIngredients()
    {
        return ingredients;
    }

    public void ClearPlate()
    {
        ingredients.Clear();
        pendingModifier = null;
    }
}
