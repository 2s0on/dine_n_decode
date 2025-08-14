using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public float patience = 30f;
    public UnityEngine.UI.Slider patienceSlider;

    [HideInInspector] public string englishOrder;
    [HideInInspector] public List<string> requiredIngredients;

    public GameObject correctIconPrefab; // assign a green checkmark prefab
    public GameObject wrongIconPrefab;   // assign a red X prefab

    private bool isServed = false;
    private float patienceTimer;
    private OrderGenerator orderGenerator;

    void Start()
    {
        patienceTimer = patience;

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

        patienceTimer -= Time.deltaTime; // slider for timer
        if (patienceSlider != null)
            patienceSlider.value = patienceTimer / patience;

        if (patienceTimer <= 0)
        {
            Debug.Log("Customer left due to impatience!");
        }
    }

    void GenerateOrder()
    {
        englishOrder = orderGenerator.GenerateEnglishOrder();
        requiredIngredients = ParseIngredientsFromOrder(englishOrder);
    }

    void DisplayOrder()
    {
        string alienOrder = AlienDictionary.TranslateOrder(englishOrder);
        if (orderText != null)
            orderText.text = alienOrder;
    }

    // cuts string, returns ingredients list
    public List<string> ParseIngredientsFromOrder(string order)
    {
        var result = new List<string>();
        if (string.IsNullOrWhiteSpace(order)) return result;

        var words = order.Split(' ');
        if (words.Length < 2) return result;

        string item = words[0];   // e.g. Burger
        string verb = words[1];   // e.g. Add / Remove (Double/Triple disabled for now)

        // Start with the base ingredients of the item
        if (AlienDictionary.ItemIngredients.TryGetValue(item, out var baseList))
            result.AddRange(baseList);

        // If there is no extra info beyond item+verb, we’re done
        if (words.Length < 3) return result;

        // Try to read optional modifier + ingredient at the end of the order.
        // Expected shapes:
        //   "Item Add Ingredient"
        //   "Item Add Modifier Ingredient"
        //   "Item Remove Ingredient"
        //   "Item Remove Modifier Ingredient"   (we’ll remove either the modded or the plain ingredient)
        string ingredient = words[^1];                  // last word
        string maybeModifier = words.Length >= 4 ? words[^2] : null;
        bool hasModifier = !string.IsNullOrEmpty(maybeModifier) &&
                           AlienDictionary.ModifierDict.ContainsKey(maybeModifier);

        string moddedName = hasModifier ? $"{maybeModifier} {ingredient}" : ingredient;

        if (verb == "Add")
        {
            // Add the requested ingredient (modded or not)
            if (!result.Contains(moddedName)) result.Add(moddedName);
        }
        else if (verb == "Remove")
        {
            // Try to remove the exact modded name; if not present, remove the plain ingredient
            if (!result.Remove(moddedName))
                result.Remove(ingredient);
        }

        return result;
    }



    // checks if the plate matches the required ingredients
    public bool CheckPlate(List<string> plateIngredients)
    {
        if (plateIngredients == null) return false;

        // Normalize (trim + case-insensitive)
        string Norm(string s) => (s ?? "").Trim();

        var reqSet = new HashSet<string>(requiredIngredients.ConvertAll(Norm), System.StringComparer.OrdinalIgnoreCase);
        var plateSet = new HashSet<string>(plateIngredients.ConvertAll(Norm), System.StringComparer.OrdinalIgnoreCase);

        // Debug help
        Debug.Log("Order (EN): " + englishOrder);
        Debug.Log("Required: " + string.Join(", ", reqSet));
        Debug.Log("Plate:    " + string.Join(", ", plateSet));

        // Show diffs
        foreach (var r in reqSet)
            if (!plateSet.Contains(r))
                Debug.LogWarning("Missing on plate: " + r);

        foreach (var p in plateSet)
            if (!reqSet.Contains(p))
                Debug.LogWarning("Extra on plate: " + p);

        return reqSet.SetEquals(plateSet);
    }


    // called when the customer is served with a plate
    public void Serve(Plate plate)
    {
        if (isServed) return;
        isServed = true;

        bool correct = CheckPlate(plate.GetIngredients());

        // spawn feedback icon above customer
        GameObject prefab = correct ? correctIconPrefab : wrongIconPrefab;
        if (prefab != null)
        {
            GameObject icon = Instantiate(prefab, transform);
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(0, 100f);
            iconRect.localScale = Vector3.one;
            Destroy(icon, 1f);
        }

        Destroy(plate.gameObject); // remove plate
        StartCoroutine(RemoveCustomerAfterDelay(1f)); // delayed by 1 second, gives time for feedback icon to show
    }


    IEnumerator RemoveCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
