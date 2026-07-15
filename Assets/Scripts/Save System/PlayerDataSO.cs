using System.Collections.Generic;
using UnityEngine;

// Gaurav Singh

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    SaveSystem Saver = new SaveSystem();

    public string SaveFile, PlayerLocation, HeldItem;
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
        HeldItem = string.Empty;

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
        HeldItem = data.HeldItem; 
        Fire = data.Fire;
        Inventory = data.Inventory;
        triggers = data.triggers;
    }

    public void AddToInventory(string item)
    {
        Debug.Log($"adding {item} to inventory");
        Inventory.Add(item);
    }

    PlayerData SOToData()
    {
        PlayerData info = new();

        info.SaveFile = SaveFile;
        info.PlayerLocation = PlayerLocation;
        info.HeldItem = HeldItem;
        info.Fire = Fire;
        info.Inventory = Inventory;
        info.triggers = triggers;

        return info;
    }

    public bool CheckSave(string FileName)
    {
        return Saver.CheckData<PlayerData>($"/{FileName}.json");
    }

    public void DeleteSave(string FileName)
    {
        Saver.DeleteData<PlayerData>($"/{FileName}.json");
    }
}
