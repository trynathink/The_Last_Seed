using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for all Inventory display related operations

    [SerializeField]
    private GameObject inventoryPanel;
    private Transform itemContainer;

    [SerializeField]
    RuntimeAnimatorController closed, open;

    [SerializeField]
    Animator anim;

    public PlayerDataSO PDSO;

    [SerializeReference]
    public Sprite defaultCursor;

    private Dictionary<CollectibleType, Sprite> spriteMap;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();

        anim.runtimeAnimatorController = closed;

        inventoryPanel.SetActive(false);
        spriteMap = new Dictionary<CollectibleType, Sprite>();
        itemContainer = inventoryPanel.transform.Find("Panel");
        //PopulateSpriteMap();
    }

    public void ToggleInventory()
    {
        Debug.Log("inventory toggled");
        bool isOpening = !inventoryPanel.activeSelf;
        if (isOpening)
        {
            GameObject.Find("Inventory Image").GetComponent<Image>().enabled = true;
            anim.runtimeAnimatorController = open;

            // clear and populate inventory
            //ClearInventory();
            //PopulateInventory();

            HoldItem(string.Empty);
            ItemsHeld();
        }
        else
        {
            GameObject.Find("Inventory Image").GetComponent<Image>().enabled = false;

            anim.runtimeAnimatorController = closed;
        }
        inventoryPanel.SetActive(isOpening);
    }

    

    void ItemsHeld()
    {
        foreach (Transform child in itemContainer)
        {
            var item = child.GetComponent<Image>();

            if (PDSO.ItemContains(child.name))
            {
                item.enabled = true;
            }
            else
            {
                item.enabled= false;
            }
        }
    }

    public void HoldItem(string item)
    {
        PDSO.HeldItem = item;

        if(item != string.Empty)
        {
            //var cursor = CreateCursor(itemContainer.Find(item).GetComponent<Image>().sprite);
            var cursor = PDSO.GetItem(item).CursorSprite;

            Cursor.SetCursor(cursor, default, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(default, default, default);
        } 
    }

    // no longer needed
    Texture2D CreateCursor(Sprite s)
    {
        int width = Mathf.FloorToInt(s.rect.width);
        int height = Mathf.FloorToInt(s.rect.height);
        Texture2D cursor = new Texture2D(width, height);

        int x = Mathf.FloorToInt(s.textureRect.x);
        int y = Mathf.FloorToInt(s.textureRect.y);

        Color[] pixels = s.texture.GetPixels(x, y, width, height);

        cursor.SetPixels(pixels);
        cursor.Apply();
        return cursor;
    }

    private void ClearInventory()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    /*
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
    */

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