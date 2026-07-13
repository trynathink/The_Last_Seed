using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    SaveSystem Saver = new SaveSystem();

    public string SaveFile, PlayerLocation, equippedItem;
    public float Fire;
    public List<string> Inventory;
    public List<string> triggers;

    public void NewGame(string FileName)
    {
        SaveFile = FileName;
        PlayerLocation = "A1 Bedroom";
        Fire = 0;
        Inventory = new List<string>();
        triggers = new List<string>();

        SaveGame();
    }

    public void SaveGame()
    {
        PlayerData data = SOToData();

        Saver.SaveData<PlayerData>($"/{data.SaveFile}.json", data);
    }

    public void LoadSave(string FileName)
    {
        PlayerData data = Saver.LoadData<PlayerData>($"/{FileName}.json");

        SaveFile = data.SaveFile;
        PlayerLocation = data.PlayerLocation;
        Fire = data.Fire;
        Inventory = data.Inventory;
        triggers = data.triggers;
        equippedItem = data.equippedItem;
    }

    public void AddToInventory(string item)
    {
        Debug.Log($"adding {item} to inventory");
        Inventory.Add(item);
    }

    public void SetEquipped(string item)
    {
        equippedItem = item;
    }

    PlayerData SOToData()
    {
        PlayerData info = new();

        info.SaveFile = SaveFile;
        info.PlayerLocation = PlayerLocation;
        info.Fire = Fire;
        info.Inventory = Inventory;
        info.triggers = triggers;
        info.equippedItem = equippedItem;

        return info;
    }

    public bool CheckSave(string FileName)
    {
        return Saver.CheckData<PlayerData>($"/{FileName}.json");
    }
}
