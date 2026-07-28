using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Clear PlayerData #&p")]
    private static void NewMenuOption()
    {
		PlayerDataSO data = Resources.FindObjectsOfTypeAll<PlayerDataSO>()[0];
		data.Clear();
		Debug.Log("Player Data Cleared!");
    }
}
