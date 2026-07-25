using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Scriptable Objects/Script")]
public class ScriptsSO : ScriptableObject
{
    public string Character;

    [SerializeReference]
    public List<string> Lines;

    public string trigger;

    public ItemSO itemGain;

    public string SceneChange;

    public ChoiceSO choice;
}
