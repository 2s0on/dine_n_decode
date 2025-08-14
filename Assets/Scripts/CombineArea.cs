using UnityEngine;

public class CombineArea : MonoBehaviour
{
    [Header("Plate Settings")]
    public GameObject platePrefab;        // Prefab reference (not actually used here, but might be used if you spawn plates dynamically)
    public RectTransform plateSpawnPoint; // Position where a plate should appear (not used in current logic)

    private GameObject currentPlateInArea; // Stores the current plate in the combine area (null if none)

    /// <summary>
    /// Called when the player drops an object onto this CombineArea.
    /// The Draggable script should trigger this.
    /// </summary>
    public void HandleDrop(GameObject dropped)
    {
        // If nothing was dropped, do nothing.
        if (dropped == null) return;

        // ---------------------------
        // CASE 1: Player dropped a Plate
        // ---------------------------
        if (dropped.CompareTag("Plate"))
        {
            // If there’s already a plate here, don’t allow another one.
            if (currentPlateInArea != null)
            {
                Debug.Log("Combine area already has a plate."); // Just a debug message
                return;
            }

            // Place the plate into this area
            dropped.transform.SetParent(transform, false); // Set this combine area as the plate's parent
            ((RectTransform)dropped.transform).anchoredPosition = Vector2.zero; // Position plate at the center of the area

            // Keep track of which plate is here
            currentPlateInArea = dropped;

            Debug.Log("Plate placed in combine area.");
            return; // Exit because we don't need to check food logic
        }

        // ---------------------------
        // CASE 2: Player dropped an Ingredient or Modifier
        // ---------------------------

        // Check if this object is a FoodItem (ingredient/modifier)
        FoodItem foodItem = dropped.GetComponent<FoodItem>();
        if (foodItem == null)
        {
            Debug.LogWarning("Dropped object has no FoodItem script!");
            return; // Can't do anything without FoodItem data
        }

        // Check if we already have a plate to put the ingredient on
        if (currentPlateInArea == null)
        {
            Debug.LogWarning("You need a plate in the combine area before adding ingredients!");
            return;
        }

        // Add the ingredient/modifier to the plate's list
        Plate plateScript = currentPlateInArea.GetComponent<Plate>();
        if (plateScript != null)
        {
            plateScript.AddFood(foodItem); // Updates the plate's stored data
        }

        // Visually move the ingredient onto the plate
        dropped.transform.SetParent(currentPlateInArea.transform, false);
        ((RectTransform)dropped.transform).anchoredPosition = Vector2.zero;

        Debug.Log($"{foodItem.itemName} placed on plate.");
    }

    /// <summary>
    /// Clears the plate reference so the area is ready for a new one.
    /// Called when plate is removed or delivered.
    /// </summary>
    public void ClearCombineArea()
    {
        currentPlateInArea = null;
    }
}
