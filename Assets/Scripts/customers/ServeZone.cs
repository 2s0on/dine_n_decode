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

            // snap plate visually to the serve zone WITHOUT reparenting to the customer
            RectTransform plateRect = draggedObj.GetComponent<RectTransform>();
            plateRect.position = transform.position + new Vector3(0, 50f, 0); // slight offset for feedback
            plateRect.localScale = Vector3.one;

            // make sure plate can still receive raycasts (interactable)
            CanvasGroup cg = draggedObj.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.blocksRaycasts = true;
                cg.alpha = 1f;
            }

            // check if the plate matches the customer's order
            bool correct = customer.CheckPlate(plate.GetIngredients());

            // call Serve to check the plate
            customer.Serve(plate);

            // reset plate back to original spot
            plate.ResetPlate();

        }
        else
        {
            Debug.LogWarning("Dropped item is not a plate or customer missing!");
        }
    }

}


