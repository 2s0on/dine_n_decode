using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    void Start()
    {
        // Call the new method to generate and display an order on start
        GenerateAndDisplayOrder();
    }

    // New method that generates an English order string
    public string GenerateEnglishOrder()
    {
        string item = GetRandomKey(AlienDictionary.ItemDict);
        string verb = GetRandomKey(AlienDictionary.VerbDict);
        string modifier = "";
        string ingredient = "";

        if (verb == "Remove")
        {
            List<string> validIngredients = AlienDictionary.ItemIngredients[item];
            ingredient = validIngredients[Random.Range(0, validIngredients.Count)];

            modifier = ""; // Optional to skip modifiers for "Remove"
        }
        else
        {
            modifier = GetRandomKey(AlienDictionary.ModifierDict);
            ingredient = GetRandomKey(AlienDictionary.IngredientDict);
        }

        string englishOrder;

        if (verb == "Remove")
        {
            englishOrder = $"{item} {verb} {ingredient}";
        }
        else
        {
            englishOrder = $"{item} {verb} {modifier} {ingredient}";
        }

        return englishOrder;
    }

    // Method to generate the order, translate it, display it, and log it
    public void GenerateAndDisplayOrder()
    {
        string englishOrder = GenerateEnglishOrder();
        string alienOrder = AlienDictionary.TranslateOrder(englishOrder);

        orderText.text = alienOrder;
        Debug.Log("English: " + englishOrder);
        Debug.Log("Alien: " + alienOrder);
    }

    private string GetRandomKey(System.Collections.Generic.Dictionary<string, string> dict)
    {
        return dict.Keys.ElementAt(Random.Range(0, dict.Count));
    }
}
