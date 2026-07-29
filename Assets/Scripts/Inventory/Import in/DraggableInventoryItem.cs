using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Vinayak Karuppasamy

// Dragging behaviour for crafting and keeping track of
// - item scriptable objects
// - inventory position

public class DraggableInventoryItem : 
    MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO itemSo;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;

    private bool droppedOnCraftingArea;

    // inventory position related info
    private Transform inventory;
    private Vector2 inventoryPosition;
    private Vector2 inventorySize;
    private Vector2 inventoryAnchorMin;
    private Vector2 inventoryAnchorMax;
    private Vector2 inventoryPivot;
    private Vector3 inventoryScale;
    private int inventorySiblingIndex;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();

        SaveInventoryPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedOnCraftingArea = false;
        image.raycastTarget = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        if (!droppedOnCraftingArea)
        {
            ReturnToInventory();
        }
    }

    private void SaveInventoryPosition()
    {
        inventory = transform.parent;
        inventoryPosition = rectTransform.anchoredPosition;
        inventorySize = rectTransform.sizeDelta;
        inventoryAnchorMin = rectTransform.anchorMin;
        inventoryAnchorMax = rectTransform.anchorMax;
        inventoryPivot = rectTransform.pivot;
        inventoryScale = rectTransform.localScale;
        inventorySiblingIndex = transform.GetSiblingIndex();
    }

    public void ReturnToInventory()
    {
        transform.SetParent(inventory, false);

        rectTransform.anchoredPosition = inventoryPosition;

        rectTransform.anchorMin = inventoryAnchorMin;
        rectTransform.anchorMax = inventoryAnchorMax;
        rectTransform.pivot = inventoryPivot;
        rectTransform.sizeDelta = inventorySize;
        rectTransform.localScale = inventoryScale;

        transform.SetSiblingIndex(inventorySiblingIndex);
    }

    public void MoveToCraftingArea(Transform craftingArea, Vector2 craftingSize)
    {
        droppedOnCraftingArea = true;

        transform.SetParent(craftingArea, false);

        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = craftingSize;
    }
}