using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choice", menuName = "Scriptable Objects/Choice")]
public class ChoiceSO : ScriptableObject
{
    public List<string> Choices;

    public List<ScriptsSO> Outcomes;
}
