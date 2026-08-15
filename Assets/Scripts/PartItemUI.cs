using UnityEngine;
using UnityEngine.EventSystems;

public class PartItemUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private PartData partData;

    public PartData PartData => partData;

    public void SetPartData(PartData data)
    {
        partData = data;
    }
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Transform originalParent;
    private Vector3 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("① PartTest - Begin Drag");

        originalParent = transform.parent;
        originalPosition = rectTransform.position;

        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("② PartTest - Drag");

        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("③ PartTest - End Drag");

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.SetParent(originalParent);
        rectTransform.position = originalPosition;
    }
}