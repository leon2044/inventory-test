using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public InventoryUI inventoryUI;
    public DragObject item;
    public int slotIndex;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(originalParent.parent);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryUI.inventoryPanel as RectTransform, eventData.position))
        {
            inventoryUI.RemoveItem(slotIndex);
        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        ItemSlotHandler droppedItem = eventData.pointerDrag.GetComponent<ItemSlotHandler>();

        if (droppedItem != null && droppedItem != this)
        {
            inventoryUI.SwapItems(slotIndex, droppedItem.slotIndex);
        }
    }
}
