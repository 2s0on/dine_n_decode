using UnityEngine;
using UnityEngine.EventSystems;

public class TrashArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj == null) return; // exit if nothing was dragged

        // check if item belongs to a plate and remove it from plate ingredients
        Plate parentPlate = draggedObj.GetComponentInParent<Plate>();
        if (parentPlate != null)
        {
            FoodItem foodItem = draggedObj.GetComponent<FoodItem>();
            if (foodItem != null)
                parentPlate.RemoveIngredient(foodItem.itemName); // remove from plate
        }

        // respawn a fresh copy of the item at its original position
        Draggable drag = draggedObj.GetComponent<Draggable>();
        if (drag != null && drag.originalParent != null)
        {
            GameObject newItem = Instantiate(draggedObj, drag.originalParent); // duplicate

            if (newItem.TryGetComponent<RectTransform>(out RectTransform rt))
                rt.anchoredPosition = drag.originalPosition; // reset position

            if (newItem.TryGetComponent<Draggable>(out Draggable newDrag))
            {
                newDrag.originalParent = drag.originalParent; // keep original parent
                newDrag.originalPosition = drag.originalPosition; // keep original pos
            }

            if (newItem.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = 1f; // make visible
                cg.blocksRaycasts = true; // make interactable
            }
        }

        // destroy the dragged object
        Destroy(draggedObj);

        Debug.Log("item trashed and respawned at original spot!");
    }
}