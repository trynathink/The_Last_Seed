using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for all Inventory display related operations

    [SerializeField]
    private GameObject inventoryPanel;
    private Transform itemContainer;

    public PlayerDataSO PDSO;

    private Dictionary<CollectibleType, Sprite> spriteMap;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
        spriteMap = new Dictionary<CollectibleType, Sprite>();
        itemContainer = inventoryPanel.transform.Find("Panel");
        PopulateSpriteMap();
    }

    public void ToggleInventory()
    {
        Debug.Log("inventory toggled");
        bool isOpening = !inventoryPanel.activeSelf;
        if (isOpening)
        {
            // clear and populate inventory
            ClearInventory();
            PopulateInventory();
        }
        inventoryPanel.SetActive(isOpening);
    }

    private void ClearInventory()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateInventory()
    {
        foreach (string item in PDSO.Inventory)
        {
            if (Enum.TryParse<CollectibleType>(item, out CollectibleType t))
            {
                AddToInventory(t);
            }
            else
            {
                Debug.Log($"Invalid collectible type recorded: {item}");
            }
        }
    }

    private void AddToInventory(CollectibleType collectible)
    {
        Sprite sprite = spriteMap[collectible];

        GameObject iconObject = new GameObject(
            collectible.ToString(),
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image)
        );

        iconObject.transform.SetParent(itemContainer, false);

        Image image = iconObject.GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
    }

    private void PopulateSpriteMap()
    {
        Sprite blanketSprite = Resources.Load<Sprite>("cropped_blanket");
        spriteMap.Add(CollectibleType.Blanket, blanketSprite);

        Sprite crowbarSprite = Resources.Load<Sprite>("cropped_crowbar");
        spriteMap.Add(CollectibleType.Crowbar, crowbarSprite);
    }
}