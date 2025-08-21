using UnityEngine;
using UnityEngine.EventSystems;

public class CombineArea : MonoBehaviour, IDropHandler
{
    [Header("plate settings")]
    public GameObject platePrefab; // prefab for new plate
    public RectTransform plateSpawnPoint; // where new plate spawns

    private GameObject currentPlateInArea; // currently active plate in combine area

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
                if (draggedObj.TryGetComponent<RectTransform>(out RectTransform plateRect))
                {
                    plateRect.SetParent(transform, false);
                    plateRect.anchoredPosition = Vector2.zero; // center in combine area
                }

                // assign current plate before any resets
                currentPlateInArea = draggedObj;
            }

            currentPlateInArea = draggedObj; // store plate reference
            Debug.Log("plate placed in combine area.");
            return;
        }

        // ingredient & modifier logic
        if (!draggedObj.TryGetComponent<FoodItem>(out FoodItem foodItem))
        {
            Debug.LogWarning("dropped object has no fooditem script!");
            return;
        }

        if (currentPlateInArea == null) // require plate first
        {
            Debug.LogWarning("you need a plate in the combine area before adding ingredients!");
            return;
        }

        // add ingredient/modifier to the current plate
        if (currentPlateInArea.TryGetComponent<Plate>(out Plate plateScript))
        {
            plateScript.AddIngredient(foodItem.itemName);
        }

        // snap ingredient visually onto plate
        if (draggedObj.TryGetComponent<RectTransform>(out RectTransform ingredientRect))
        {
            ingredientRect.SetParent(currentPlateInArea.transform, false);
            ingredientRect.anchoredPosition = Vector2.zero;
        }

        Debug.Log($"{foodItem.itemName} placed on plate.");

        // reset draggable back to original spot
        if (draggedObj.TryGetComponent<Draggable>(out Draggable draggable) && draggable.originalParent != null)
        {
            // instantiate a new ingredient at original parent
            GameObject newIngredient = Instantiate(draggedObj.gameObject, draggable.originalParent);
            if (newIngredient.TryGetComponent<RectTransform>(out RectTransform newRect))
            {
                newRect.anchoredPosition = draggable.originalPosition; // snap to original
            }

            if (newIngredient.TryGetComponent<Draggable>(out Draggable newDraggable))
            {
                newDraggable.originalParent = draggable.originalParent; // keep original parent
                newDraggable.originalPosition = draggable.originalPosition; // keep original pos
            }

            if (newIngredient.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = 1f; // reset alpha
                cg.blocksRaycasts = true; // enable raycasts
            }
        }
    }

}
