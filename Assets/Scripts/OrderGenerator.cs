using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    void Start()
    {
        string item = GetRandomKey(AlienDictionary.ItemDict);
        string verb = GetRandomKey(AlienDictionary.VerbDict);
        string modifier = "";
        string ingredient = "";

        if (verb == "Remove")
        {
            List<string> validIngredients = AlienDictionary.ItemIngredients[item];
            ingredient = validIngredients[Random.Range(0, validIngredients.Count)];

            // Optional: still allow modifiers only if you want!
            modifier = ""; // You can skip modifiers for "Remove" or handle specially
        }
        else
        {
            modifier = GetRandomKey(AlienDictionary.ModifierDict);
            ingredient = GetRandomKey(AlienDictionary.IngredientDict);
        }

        string englishOrder;

        if (verb == "Remove")
        {
            // Skip the modifier when removing
            englishOrder = $"{item} {verb} {ingredient}";
        }
        else
        {
            englishOrder = $"{item} {verb} {modifier} {ingredient}";
        }

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
