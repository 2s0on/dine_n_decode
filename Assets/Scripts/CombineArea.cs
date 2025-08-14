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
        Debug.Log("Combine area already has a plate."); // prevents user from placing multiple plates
        return;
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

    // clears the combine area, removing the current plate
    // !! Not sure if this works !!
    public void ClearCombineArea()
{
    currentPlateInArea = null;
}
}
