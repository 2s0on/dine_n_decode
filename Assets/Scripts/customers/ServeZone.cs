using UnityEngine;
using UnityEngine.EventSystems;

public class ServeZone : MonoBehaviour, IDropHandler
{
    [Header("Customer linked to this zone")]
    public Customer customer; // assign via inspector


    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj == null) return;

        Plate plate = draggedObj.GetComponent<Plate>();
        if (plate != null && customer != null)
        {
            Debug.Log("Plate dropped in serve zone!");

            // snap plate to zone visually
            RectTransform plateRect = draggedObj.GetComponent<RectTransform>();
            plate.SetParentAndKeepScale(transform);
            plateRect.anchoredPosition = Vector2.zero;
            // prevents plate from scaling when parented to this zone

            // disable dragging
            Draggable dragScript = draggedObj.GetComponent<Draggable>();
            if (dragScript != null) Destroy(dragScript);

            // called for customer.cs to check plate
            customer.Serve(plate);
            CombineArea combineArea = FindObjectOfType<CombineArea>();
            if (combineArea != null)
            {
                combineArea.ResetCombineArea();
            }
        }
        else
        {
            Debug.LogWarning("Dropped item is not a plate or customer missing!");
        }
    }
}