using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public TextMeshProUGUI orderText; // UI text to show alien order
    public float patience = 30f; // seconds before customer leaves
    private float patienceTimer;
    public UnityEngine.UI.Slider patienceSlider;

    [HideInInspector] public string englishOrder;
    [HideInInspector] public List<string> requiredIngredients;

    private bool isServed = false;

    private OrderGenerator orderGenerator;

    void Start()
    {
        patienceTimer = patience;

        // Option 1: Get the order from an existing OrderGenerator instance
        orderGenerator = Object.FindFirstObjectByType<OrderGenerator>();

        if (orderGenerator != null)
        {
            GenerateOrder();
            DisplayOrder();
        }
        else
        {
            Debug.LogError("OrderGenerator not found in scene!");
        }
    }

    void Update()
    {
        if (isServed) return;

        patienceTimer -= Time.deltaTime;

        if (patienceSlider != null)
            patienceSlider.value = patienceTimer / patience;

        if (patienceTimer <= 0)
        {
            Debug.Log("Customer left due to impatience!");
            LeaveCustomer(false);
        }
    }


    void GenerateOrder()
    {
        // You can either generate a fresh order string here
        // or reuse the logic inside OrderGenerator.cs for consistency

        // Example (simplified):
        englishOrder = orderGenerator.GenerateEnglishOrder();
        requiredIngredients = ParseIngredientsFromOrder(englishOrder);
    }

    void DisplayOrder()
    {
        string alienOrder = AlienDictionary.TranslateOrder(englishOrder);
        if (orderText != null)
            orderText.text = alienOrder;
    }

    // This is where you parse the englishOrder string into ingredient list to compare later
    List<string> ParseIngredientsFromOrder(string order)
    {
        // For example: "Burger Add Spicy Patty"
        // We want to find out the base item, action, modifier, ingredient
        // and produce a list of ingredients that represent the correct plate

        List<string> ingredients = new List<string>();

        string[] words = order.Split(' ');

        // Simple example for Add action:
        // First word is item, last word is ingredient, modifier in the middle
        string item = words[0];
        string verb = words[1];

        // Start with base ingredients of the item
        if (AlienDictionary.ItemIngredients.ContainsKey(item))
            ingredients.AddRange(AlienDictionary.ItemIngredients[item]);

        if (verb == "Add" && words.Length >= 4)
        {
            // Add modifier and ingredient if applicable
            string modifier = words[2];
            string ingredient = words[3];

            // Depending on your game logic, you might want to add ingredient here
            if (!ingredients.Contains(ingredient))
                ingredients.Add(ingredient);

            // You might also handle modifiers affecting ingredients or properties
        }
        else if (verb == "Remove" && words.Length >= 3)
        {
            string ingredientToRemove = words[2];
            ingredients.Remove(ingredientToRemove);
        }
        // You can expand to handle Double, Triple verbs as well

        return ingredients;
    }

    // Call this to check if plate ingredients match customer order
    public bool CheckPlate(List<string> plateIngredients)
    {
        // Simplest matching: check if lists contain same ingredients (ignoring order)
        if (plateIngredients == null) return false;

        // Check counts first
        if (plateIngredients.Count != requiredIngredients.Count)
            return false;

        // Check that all required ingredients are present in plate
        foreach (string reqIng in requiredIngredients)
        {
            if (!plateIngredients.Contains(reqIng))
                return false;
        }

        return true;
    }

    // Call this when player serves a plate
    public void Serve(List<string> plateIngredients)
    {
        if (CheckPlate(plateIngredients))
        {
            Debug.Log("Customer served correctly!");
            isServed = true;
            LeaveCustomer(true);
        }
        else
        {
            Debug.Log("Customer served incorrectly!");
            // TODO: Handle failure - maybe lose points or show error
        }
    }

    void LeaveCustomer(bool success)
    {
        // TODO: Add animations or effects here

        // For now just destroy customer GameObject
        Destroy(gameObject);
    }
}
