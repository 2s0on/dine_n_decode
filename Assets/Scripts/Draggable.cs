using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas; // canvas to use for drag movement, can be auto-assigned

    private RectTransform rectTransform; // stores the rect transform of this object
    private CanvasGroup canvasGroup;     // used to control alpha and raycast blocking

    public Vector2 originalPosition; // stores original anchored position
    public Transform originalParent; // stores original parent to reset after drag

    private Vector2 pointerOffset; // offset from pointer to object center for smooth dragging

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // get rect transform
        canvasGroup = GetComponent<CanvasGroup>();     // get canvas group for alpha/raycast control
        originalParent = transform.parent;            // store parent for reset
        originalPosition = rectTransform.anchoredPosition; // store initial position

        // auto-assign canvas if missing
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // visually indicate that the object is being pressed
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // allow raycasts to pass through while dragging

        // calculate offset between pointer and object position for smooth dragging
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset
        );

        // temporarily move the object to the canvas to avoid hierarchy issues
        rectTransform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        // convert screen point to local canvas point and apply offset
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            rectTransform.localPosition = localPoint - pointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;          // reset visual alpha
        canvasGroup.blocksRaycasts = true; // re-enable raycast blocking
    }
}
