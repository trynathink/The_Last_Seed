using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombinedItem", menuName = "Scriptable Objects/CombinedItemSO")]
public class CombinedItemSO : ItemSO
{
    public ItemSO Ingredient1;

    public ItemSO Ingredient2;
}