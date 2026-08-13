using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for all Inventory display related operations

    [SerializeField]
    private GameObject inventoryPanel, informationPanel;
    [SerializeField]
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
        anim = transform.GetChild(1).GetComponent<Animator>();

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

    

    public void ItemsHeld()
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
            Cursor.SetCursor(default, default, CursorMode.ForceSoftware);
        } 
    }

    private void ClearInventory()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void ToggleInfo()
    {
        informationPanel.SetActive(!informationPanel.activeSelf);
    }

    public List<ItemSO> GetAllItems()
    {   
        List<ItemSO> items = new List<ItemSO>();

        foreach(RectTransform child in itemContainer)
        {
            items.Add(child.GetComponent<DraggableInventoryItem>().itemSo);

        }

        return items;


    }
}