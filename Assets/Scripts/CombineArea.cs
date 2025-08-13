using UnityEngine;
using UnityEngine.EventSystems;

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

        // Plate logic
        if (draggedObj.CompareTag("Plate"))
        {
            if (currentPlateInArea != null)
            {
                Debug.Log("Combine area already has a plate.");
                return;
            }

            // Snap plate to CombineArea
            RectTransform droppedRect = draggedObj.GetComponent<RectTransform>();
            droppedRect.SetParent(transform, false);
            droppedRect.anchoredPosition = Vector2.zero;


            currentPlateInArea = draggedObj;
            Debug.Log("Plate placed in combine area.");
            return; // Stop here for plates
        }

        // Ingredient logic
        if (draggedObj.CompareTag("Ingredient"))
        {
            if (currentPlateInArea == null)
            {
                Debug.LogWarning("You need a plate in the combine area before adding ingredients!");
                return;
            }

            // Snap ingredient onto the plate
            RectTransform droppedRect = draggedObj.GetComponent<RectTransform>();
            droppedRect.SetParent(currentPlateInArea.transform, false);
            droppedRect.anchoredPosition = Vector2.zero; // or offset if needed

            Debug.Log("Ingredient placed on plate.");

            // Spawn new ingredient at its original spot
            Draggable draggable = draggedObj.GetComponent<Draggable>();
            if (draggable != null && draggable.originalParent != null)
            {
                GameObject newIngredient = Instantiate(draggedObj.gameObject, draggable.originalParent);
                RectTransform newRect = newIngredient.GetComponent<RectTransform>();
                newRect.anchoredPosition = draggable.originalPosition;

                // Reset Draggable info so the new one works properly
                Draggable newDraggable = newIngredient.GetComponent<Draggable>();
                newDraggable.originalParent = draggable.originalParent;
                newDraggable.originalPosition = draggable.originalPosition;

                // Ensure the canvas group is reset so it can be dragged
                CanvasGroup cg = newIngredient.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = 1f;
                    cg.blocksRaycasts = true;
                }
            }
        }
    }

    public void ClearCombineArea()
    {
        currentPlateInArea = null;
    }
}

