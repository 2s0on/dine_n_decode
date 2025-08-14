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
            plateRect.SetParent(transform, false);
            plateRect.anchoredPosition = Vector2.zero;

            // disable dragging
            Draggable dragScript = draggedObj.GetComponent<Draggable>();
            if (dragScript != null) Destroy(dragScript);

            // called for customer.cs to check plate
            customer.Serve(plate);
        }
        else
        {
            Debug.LogWarning("Dropped item is not a plate or customer missing!");
        }
    }
}