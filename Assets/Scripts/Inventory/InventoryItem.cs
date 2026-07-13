using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    private CollectibleType type;
    private InventoryManager manager;

    public void Initialize(CollectibleType collectibleType, InventoryManager inventoryManager)
    {
        type = collectibleType;
        manager = inventoryManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.SetSelectedItem(type);
    }
}