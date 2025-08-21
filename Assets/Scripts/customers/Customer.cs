using System; // add this namespace for action
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public TextMeshProUGUI orderText; // text ui to display alien order
    public float patience = 30f; // max patience time
    public UnityEngine.UI.Slider patienceSlider; // slider ui for patience

    [HideInInspector] public string englishOrder; // store original order
    [HideInInspector] public List<string> requiredIngredients; // store ingredients needed

    public GameObject correctIconPrefab; // green checkmark prefab
    public GameObject wrongIconPrefab;   // red x prefab
    public event Action onCustomerLeave; // event when customer leaves

    private bool isServed = false; // flag to prevent double serving
    private float patienceTimer;
    private OrderGenerator orderGenerator; // reference to order generator

    void Start()
    {
        patienceTimer = patience; // start timer

        orderGenerator = UnityEngine.Object.FindFirstObjectByType<OrderGenerator>();
        if (orderGenerator != null)
        {
            // generate order once and store
            englishOrder = orderGenerator.GenerateEnglishOrder();
            requiredIngredients = ParseIngredientsFromOrder(englishOrder);

            DisplayOrder(); // show translated alien order
        }
    }

    void Update()
    {
        if (isServed) return; // stop timer if already served

        patienceTimer -= Time.deltaTime; // decrement timer
        if (patienceSlider != null)
            patienceSlider.value = patienceTimer / patience; // update slider

        if (patienceTimer <= 0 && !isServed)
        {
            isServed = true; // prevent multiple triggers
            Debug.Log("customer left due to impatience!");
            StartCoroutine(RemoveCustomerAfterDelay(0f)); // remove immediately
        }
    }

    void DisplayOrder()
    {
        string alienOrder = AlienDictionary.TranslateOrder(englishOrder); // translate order
        if (orderText != null)
            orderText.text = alienOrder; // update ui
    }

    // parse english order into ingredient list
    public List<string> ParseIngredientsFromOrder(string order)
    {
        var result = new List<string>();
        if (string.IsNullOrWhiteSpace(order)) return result;

        var words = order.Split(' ');
        if (words.Length < 2) return result;

        string item = words[0];   // first word is item
        string verb = words[1];   // second word is add/remove

        // add base ingredients from dictionary
        if (AlienDictionary.ItemIngredients.TryGetValue(item, out var baseList))
            result.AddRange(baseList);

        if (words.Length < 3) return result; // done if no extra words

        // everything after verb
        var extraWords = new List<string>(words).GetRange(2, words.Length - 2);

        if (verb == "Add")
        {
            foreach (var w in extraWords)
            {
                if (!result.Contains(w))
                    result.Add(w); // add extra ingredients
            }
        }
        else if (verb == "Remove")
        {
            foreach (var w in extraWords)
            {
                result.Remove(w); // remove specified ingredients
            }
        }

        return result;
    }

    // check if plate matches this customer's required ingredients
    public bool CheckPlate(List<string> plateIngredients)
    {
        if (plateIngredients == null) return false;

        // normalize strings
        string Norm(string s) =>
            (s ?? "")
                .Trim() // remove spaces
                .Replace("\u200B", "") // remove zero-width spaces
                .Replace("\n", "")     // remove line breaks
                .Replace("\r", "")     // remove carriage returns
                .ToLower();            // lowercase

        var reqSet = new HashSet<string>(requiredIngredients.ConvertAll(Norm), System.StringComparer.OrdinalIgnoreCase);
        var plateSet = new HashSet<string>(plateIngredients.ConvertAll(Norm), System.StringComparer.OrdinalIgnoreCase);

        // debug logs
        Debug.Log("order (en): " + englishOrder);
        Debug.Log("required: " + string.Join(", ", reqSet));
        Debug.Log("plate:    " + string.Join(", ", plateSet));

        // log missing items
        foreach (var r in reqSet)
            if (!plateSet.Contains(r))
                Debug.LogWarning("missing on plate: " + r);

        // log extra items
        foreach (var p in plateSet)
            if (!reqSet.Contains(p))
                Debug.LogWarning("extra on plate: " + p);

        return reqSet.SetEquals(plateSet); // return if sets are equal
    }

    // serve plate to customer
    public void Serve(Plate plate)
    {
        if (isServed) return;
        isServed = true;

        // check plate ingredients against stored required ingredients
        bool correct = CheckPlate(plate.GetIngredients());

        // spawn feedback icon above customer
        GameObject prefab = correct ? correctIconPrefab : wrongIconPrefab;
        if (prefab != null)
        {
            GameObject icon = Instantiate(prefab, transform);
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(0, 100f); // position above customer
            iconRect.localScale = Vector3.one;
            Destroy(icon, 1f); // remove after 1 second
        }

        StartCoroutine(RemoveCustomerAfterDelay(1f)); // delay before removing customer
    }

    // remove customer after delay
    IEnumerator RemoveCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        onCustomerLeave?.Invoke(); // notify manager
        Destroy(gameObject); // destroy customer
    }
}
