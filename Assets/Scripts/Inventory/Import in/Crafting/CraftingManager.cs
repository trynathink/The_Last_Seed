using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Vinayak Karuppasamy

// Crafting related operations go here

public class CraftingManager : MonoBehaviour
{
    [SerializeField]
    private Transform craftingArea;

    private InventoryManager inventoryManager;

    public PlayerDataSO playerData;

    // read-only
    private static Dictionary<(string, string), CombinedItemSO> validCombinations;
    private static HashSet<string> combinableItems;

    private void Awake()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    private void Start()
    {
        PopulateValidCombinations();
    }

    public void Craft()
    {
        if(CraftMeter() != 2)
        {
            Debug.Log("invalid state - button should not have been enabled");
            return;
        }

        Debug.Log("Crafting!");

        string leftName = craftingArea.GetChild(0).name;
        string rightName = craftingArea.GetChild(1).name;

        CombinedItemSO combinedItem = validCombinations[MakePair(leftName, rightName)];

        // remove ingredients from player inventory
        playerData.Inventory.RemoveAll(
            item => item.Name == leftName || item.Name == rightName);
        Debug.Log($"Removed {leftName} and {rightName} from inventory");

        // return crafting area ingredients to inventory
        craftingArea.GetChild(0).GetComponent<DraggableInventoryItem>().ReturnToInventory();
        craftingArea.GetChild(0).GetComponent<DraggableInventoryItem>().ReturnToInventory();

        // add combined item to inventory
        playerData.Inventory.Add(combinedItem);
        Debug.Log($"Added {combinedItem.Name} to inventory");
        
        // reset inventory display
        inventoryManager.ItemsHeld();
    }

    // 0 - no items OR left is uncombinable OR both are incompatible
    // 1 - one item - left is combinable OR left is combinable but right isn't compatible
    // 2 - both items are present and valid
    public int CraftMeter()
    {
        if(craftingArea.childCount > 0 && craftingArea.childCount < 3)
        {
            string leftName = craftingArea.GetChild(0).name;
            
            if(!combinableItems.Contains(leftName))
                return 0;

            if(craftingArea.childCount == 2)
            {
                string rightName = craftingArea.GetChild(1).name;
                if (validCombinations.ContainsKey(MakePair(leftName, rightName)))
                    return 2;
            }
            
            return 1;
        }

        return 0;
    }

    private void PopulateValidCombinations()
    {
        if(validCombinations != null && validCombinations.Count != 0)
            return;
        
        validCombinations = new();
        // load all valid combinations from combined items defined in inventory
        List<CombinedItemSO> combinedItems = inventoryManager.GetAllItems()
                                        .OfType<CombinedItemSO>()
                                        .ToList();
        
        foreach(CombinedItemSO item in combinedItems)
        {
            validCombinations[MakePair(item.Ingredient1.Name, item.Ingredient2.Name)] = item;
            Debug.Log(
                $"valid combination: {item.Ingredient1.Name} and {item.Ingredient2.Name} for {item.Name}");
        }

        PopulateCombinableItems();
    }

    private void PopulateCombinableItems()
    {
        if(combinableItems != null && combinableItems.Count != 0)
            return;

        combinableItems = new();
        combinableItems = new HashSet<string>(
            validCombinations.Keys.SelectMany(k => new[]{ k.Item1, k.Item2 })
        );
    }

    private static (string, string) MakePair(string a, string b)
    {
        return string.CompareOrdinal(a, b) <= 0 ? (a, b) : (b, a);
    }

}