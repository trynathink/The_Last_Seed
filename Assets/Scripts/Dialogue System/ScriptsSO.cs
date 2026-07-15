using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Scriptable Objects/Script")]
public class ScriptsSO : ScriptableObject
{
    public string Character;

    [SerializeReference]
    public List<string> Lines;

    // Defunct
    /*[SerializeField]
    public List<int> NPCplacement, WordCount;*/

    public string trigger, itemGain;

    public string SceneChange;

    public ChoiceSO choice;
}
