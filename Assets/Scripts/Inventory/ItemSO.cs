using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string Name;

    public Texture2D CursorSprite;

    public byte[] CursorBytes;

}
