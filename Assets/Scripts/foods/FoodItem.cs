using UnityEngine;

public enum FoodType //enumeration for food item types
{
    Ingredient, // e.g., Patty, Lettuce, Buns
    Modifier    // e.g., Spicy, Sweet, Salty
}

public class FoodItem : MonoBehaviour
{
    [Header("Food Info")]
    public FoodType type;   // choose ingredient or modifier in inspector
    public string itemName; // exact name, e.g., "Patty", "Lettuce", "Spicy"

    public string GetName()
    {
        return itemName;
    }
}
