using System.Collections.Generic;
// allows the use of dictionaries, not attached to any game object, no need for unityEngine or monobehaviour
public static class AlienDictionary
{
    // dictionary for food items
    public static Dictionary<string, string> ItemDict = new Dictionary<string, string>()
    {
        {"Burger", "Targo"},
        {"Salad", "Voola"},
        //{"Pasta", "Nozu"},
        //{"Soup", "Lirpa"},
        //{"Cake", "Drovi"},
        {"Juice", "Sippa"}
    };

    // dictionary for verbs (actions)
    public static Dictionary<string, string> VerbDict = new Dictionary<string, string>()
    {
        {"Add", "Zin"},
        {"Remove", "Droka"},
        //{"Double", "Reffo"},
        //{"Triple", "Trakka"}
    };

    // dictionary for modifiers (flavors)
    public static Dictionary<string, string> ModifierDict = new Dictionary<string, string>()
    {
        {"Spicy", "Krel"},
        {"Salty", "Nash"},
        {"Sweet", "Milu"},
        //{"Peppery", "Gorza"},
        //{"Tangy", "Zintal"}
    };

    // dictionary for ingredients
    public static Dictionary<string, string> IngredientDict = new Dictionary<string, string>()
    {
        {"Buns", "Plor"},
        {"Lettuce", "Frilka"},
        {"Patty", "Zeggo"},
        {"Cheese", "Molcha"},
        {"Tomato", "Rezzi"},
        //{"Cream", "Zugu" },
        {"Fruits", "Noki" },
        //{"Noodles", "Fuzza" },
        //{"Mushroom", "Gorlo" },
        {"Ice", "Zilto" }
    };

    // dictionary for the ingredients of the food items
    public static Dictionary<string, List<string>> ItemIngredients = new Dictionary<string, List<string>>()
    {
        {"Burger", new List<string> { "Buns", "Patty", "Cheese", "Lettuce", "Tomato" } },
        {"Salad", new List<string> { "Lettuce", "Tomato", "Fruits" } },
        //{"Pasta", new List<string> { "Noodles", "Tomato", "Cheese", "Cream" } },
        //{"Soup", new List<string> { "Cream", "Mushroom" } },
        //{"Cake", new List<string> { "Cream", "Fruits" } },
        {"Juice", new List<string> { "Fruits", "Ice" } }
    };


    public static string TranslateOrder(string order)
    {
        string[] parts = order.Split(' ', ','); // split into words on spaces and commas, also lowercases everything
        List<string> translated = new List<string>(); // empty list to store alien words

        foreach (string rawWord in parts)
        {
            string word = rawWord.Trim(); // remove any extra punctuation/spaces
            if (string.IsNullOrEmpty(word)) continue;

            if (ItemDict.TryGetValue(word, out string itemAlien))
            {
                translated.Add(itemAlien); // adds to the list when found
            }
            else if (VerbDict.TryGetValue(word, out string verbAlien))
            {
                translated.Add(verbAlien);
            }
            else if (ModifierDict.TryGetValue(word, out string modAlien))
            {
                translated.Add(modAlien);
            }
            else if (IngredientDict.TryGetValue(word, out string ingAlien))
            {
                translated.Add(ingAlien);
            }
            else
            {
                translated.Add(word); // uses the original word if not found (fallback)
            }
        }

        return string.Join(" ", translated); // joins all words with spaces to return a sentence
    }


    private static string Capitalize(string word) // capitalizes the first letter of the word 
    {
        if (string.IsNullOrEmpty(word)) return word;
        return char.ToUpper(word[0]) + word.Substring(1);
    }

}
