using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PageBurnerSO", menuName = "Scriptable Objects/PageBurnerSO")]
public class PageBurnerSO : ScriptableObject
{
    public List<Sprite> Burn;
}
