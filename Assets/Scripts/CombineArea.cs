using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CombineArea : MonoBehaviour, IDropHandler
{
    [Header("Plate Settings")]
    public GameObject platePrefab;
    public RectTransform plateSpawnPoint;

    private GameObject currentPlateInArea;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj == null) return;

        // plate logic
        if (draggedObj.CompareTag("Plate"))
        {
            if (currentPlateInArea != null)
            {
                // snap plate to combine area
                RectTransform plateRect = draggedObj.GetComponent<RectTransform>();
                plateRect.anchoredPosition = Vector2.zero;

                // assign to currentPlateInArea BEFORE any reset/respawn
                currentPlateInArea = draggedObj;
            }


            RectTransform droppedRect = draggedObj.GetComponent<RectTransform>();
            droppedRect.SetParent(transform, false);
            droppedRect.anchoredPosition = Vector2.zero; // snap plate to combine area


            currentPlateInArea = draggedObj;
            Debug.Log("Plate placed in combine area.");
            return;
        }

        // ingredient & modifier logic
        FoodItem foodItem = draggedObj.GetComponent<FoodItem>();
        if (foodItem == null) // checks for FoodItem script
        {
            Debug.LogWarning("Dropped object has no FoodItem script!");
            return;
        }



        if (currentPlateInArea == null) // checks if a plate exists in the combine area
        {
            Debug.LogWarning("You need a plate in the combine area before adding ingredients!");
            return;
        }

        // add the ingredient/modifier to the plate
        Plate plateScript = currentPlateInArea.GetComponent<Plate>();
        if (plateScript != null)
        {
            plateScript.AddIngredient(foodItem.itemName);
        }

        // snap ingredient onto plate
        RectTransform ingredientRect = draggedObj.GetComponent<RectTransform>();
        ingredientRect.SetParent(currentPlateInArea.transform, false);
        ingredientRect.anchoredPosition = Vector2.zero;

        Debug.Log($"{foodItem.itemName} placed on plate.");


        // resets the draggable in original spot
        Draggable draggable = draggedObj.GetComponent<Draggable>();
        if (draggable != null && draggable.originalParent != null)
        {
            // instantiate a new ingredient at the original position
            GameObject newIngredient = Instantiate(draggedObj.gameObject, draggable.originalParent);
            RectTransform newRect = newIngredient.GetComponent<RectTransform>();
            newRect.anchoredPosition = draggable.originalPosition;

            // reset the draggable component (ensures that it can be dragged again)
            Draggable newDraggable = newIngredient.GetComponent<Draggable>();
            newDraggable.originalParent = draggable.originalParent;
            newDraggable.originalPosition = draggable.originalPosition;

            // reset the canvas group properties ensuring it can be interacted with
            CanvasGroup cg = newIngredient.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.blocksRaycasts = true;

            }
        }
    }

    // resets the combine area and respawns all items after a plate is served
    public void ResetCombineArea()
    {
        if (currentPlateInArea != null)
        {
            Plate plateComp = currentPlateInArea.GetComponent<Plate>();
            RectTransform plateRect = currentPlateInArea.GetComponent<RectTransform>();
            if (plateRect != null && plateComp != null)
            {
                // move plate back to spawn point
                plateRect.SetParent(plateSpawnPoint, false);
                plateRect.anchoredPosition = Vector2.zero;

                // force scale
                plateRect.localScale = plateComp.transform.localScale;
            }

            // Reset draggable info
            Draggable plateDrag = currentPlateInArea.GetComponent<Draggable>();
            if (plateDrag != null)
            {
                plateDrag.originalParent = plateSpawnPoint;
                plateDrag.originalPosition = Vector2.zero;
            }
        }

        // Reset all ingredients/modifiers
        Draggable[] allDraggables = FindObjectsOfType<Draggable>();
        foreach (Draggable d in allDraggables)
        {
            if (d.CompareTag("Plate")) continue; // skip plate itself

            RectTransform rect = d.GetComponent<RectTransform>();
            rect.SetParent(d.originalParent, false);
            rect.anchoredPosition = d.originalPosition;
            rect.localScale = Vector3.one;

            CanvasGroup cg = d.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.blocksRaycasts = true;
            }
        }

        // clear reference so a new plate can be used
        currentPlateInArea = null;
    }

}

