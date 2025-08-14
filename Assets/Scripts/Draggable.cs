//using UnityEngine;
//using UnityEngine.EventSystems;


//public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
//    // enables the private varaible to still be edited in the inspector, is used to scale the drag movement -- to be same as the mouse movement
//    [SerializeField] private Canvas canvas;

//    // RectTransform is the transform for UI elements in Unity
//    private RectTransform rectTransform;
//    // CanvasGroup is used to control the transparency and raycast blocking of UI elements
//    private CanvasGroup canvasGroup;

//    public Vector2 originalPosition;
//    // stores the positions of the original item, to be spawned back at the same position later
//    public Transform originalParent;

//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();
//        originalPosition = rectTransform.anchoredPosition;
//        originalParent = transform.parent;
//    }

//    public void OnPointerDown(PointerEventData eventData) {
//        Debug.Log("OnPointerDown clicked");
//        canvasGroup.alpha = 0.6f; // makes the object semi-transparent when clicked
//    }
//    public void OnDrag(PointerEventData eventData) {
//        Debug.Log("OnDrag called");
//        // moves the object according to your mouse movement
//        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
//        canvasGroup.blocksRaycasts = false; // allows the object to be dragged without blocking other UI elements
//    }
//    public void OnEndDrag(PointerEventData eventData) {
//        Debug.Log("OnEndDrag called");
//        canvasGroup.alpha = 1f; // resets the alpha to fully opaque
//        canvasGroup.blocksRaycasts = true; // allows the object to block other UI elements again
//    }
//    public void OnBeginDrag(PointerEventData eventData) {
//        Debug.Log("OnBeginDrag called");
//    }

//}


using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;

        canvasGroup.blocksRaycasts = false; // Allow raycast to pass through while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget != null)
        {
            // Check if it's a CombineZone
            var combineZone = dropTarget.GetComponent<CombineArea>();
            if (combineZone != null)
            {
                combineZone.HandleDrop(gameObject);
                return;
            }

            // Check if it's a ServeZone
            var serveZone = dropTarget.GetComponent<ServeZone>();
            if (serveZone != null)
            {
                // If ServeZone has HandleDrop, call it
                var method = serveZone.GetType().GetMethod("HandleDrop");
                if (method != null)
                {
                    method.Invoke(serveZone, new object[] { gameObject });
                }
                return;
            }

            // Check if it's a TrashZone
            var trashArea = dropTarget.GetComponent<TrashArea>();
            if (trashArea != null)
            {
                trashArea.DeleteItem(gameObject);
                return;
            }
        }

        // If dropped anywhere else, snap back
        transform.position = startPosition;
        transform.SetParent(originalParent);
    }
}
