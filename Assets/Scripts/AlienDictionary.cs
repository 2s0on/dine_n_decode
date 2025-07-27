using System.Collections.Generic;
// allows the use of dictionaries, not attached to any game object, no need for unityEngine or monobehaviour
public static class AlienDictionary
{
    public static Dictionary<string, string> ItemDict = new Dictionary<string, string>()
    {
        {"Burger", "Targo"},
        {"Salad", "Voola"},
        {"Pasta", "Nozu"},
        {"Soup", "Lirpa"},
        {"Cake", "Drovi"},
        {"Juice", "Sippa"}
    };

    public static Dictionary<string, string> VerbDict = new Dictionary<string, string>()
    {
        {"Add", "Zin"},
        {"Remove", "Droka"},
        {"Double", "Reffo"},
        {"Triple", "Trakka"}
    };

    public static Dictionary<string, string> ModifierDict = new Dictionary<string, string>()
    {
        {"Spicy", "Krel"},
        {"Salty", "Nash"},
        {"Sweet", "Milu"},
        {"Peppery", "Gorza"},
        {"Tangy", "Zintal"}
    };

    public static Dictionary<string, string> IngredientDict = new Dictionary<string, string>()
    {
        {"Bun", "Plor"},
        {"Lettuce", "Frilka"},
        {"Patty", "Zeggo"},
        {"Cheese", "Molcha"},
        {"Tomato", "Rezzi"},
        {"Cream", "Zugu" },
        {"Fruits", "Noki" },
        {"Noodles", "Fuzza" },
        {"Mushroom", "Gorlo" },
        {"Ice", "Zilto" }
    };

    public static Dictionary<string, List<string>> ItemIngredients = new Dictionary<string, List<string>>()
    {
        {"Burger", new List<string> { "Bun", "Patty", "Cheese", "Lettuce", "Tomato" } },
        {"Salad", new List<string> { "Lettuce", "Tomato", "Fruits" } },
        {"Pasta", new List<string> { "Noodles", "Tomato", "Cheese", "Cream" } },
        {"Soup", new List<string> { "Cream", "Mushroom" } },
        {"Cake", new List<string> { "Cream", "Fruits" } },
        {"Juice", new List<string> { "Fruits", "Ice" } }
    };

    public static string TranslateOrder(string order)
    {
        string[] parts = order.ToLower().Split(' ', ','); // split on spaces and commas
        List<string> translated = new List<string>();

        foreach (string rawWord in parts)
        {
            string word = rawWord.Trim(); // remove any extra punctuation/spaces
            if (string.IsNullOrEmpty(word)) continue;

            if (ItemDict.TryGetValue(Capitalize(word), out string itemAlien))
            {
                translated.Add(itemAlien);
            }
            else if (VerbDict.TryGetValue(Capitalize(word), out string verbAlien))
            {
                translated.Add(verbAlien);
            }
            else if (ModifierDict.TryGetValue(Capitalize(word), out string modAlien))
            {
                translated.Add(modAlien);
            }
            else if (IngredientDict.TryGetValue(Capitalize(word), out string ingAlien))
            {
                translated.Add(ingAlien);
            }
            else
            {
                translated.Add(word); // fallback to original if not found
            }
        }

        return string.Join(" ", translated);
    }

    private static string Capitalize(string word)
    {
        if (string.IsNullOrEmpty(word)) return word;
        return char.ToUpper(word[0]) + word.Substring(1);
    }

}
