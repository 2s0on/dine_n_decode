using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    void Start()
    {
        // call the new method to generate and display an order on start
        GenerateAndDisplayOrder();
    }

    // generate an english order string
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
            modifier = ""; // skips modifier for Remove
        }
        else // Add verb
        {
            ingredient = GetRandomKey(AlienDictionary.IngredientDict);
            modifier = GetRandomKey(AlienDictionary.ModifierDict); // modifier always applied on Add
        }

        // Format order based on verb
        if (verb == "Remove")
            return $"{item} {verb} {ingredient}";
        else
            return $"{item} {verb} {modifier} {ingredient}";
    }


    // generate the order, translate it, display it, and log it
    public void GenerateAndDisplayOrder()
    {
        string englishOrder = GenerateEnglishOrder();
        string alienOrder = AlienDictionary.TranslateOrder(englishOrder);

        orderText.text = alienOrder;
        Debug.Log("English: " + englishOrder);
        Debug.Log("Alien: " + alienOrder);
    }

    // calls on alien dictionary to get random keys to generate orders
    private string GetRandomKey(System.Collections.Generic.Dictionary<string, string> dict)
    {
        return dict.Keys.ElementAt(Random.Range(0, dict.Count));
    }
}
