using System.Collections.Generic;
using UnityEngine;

// Gaurav Singh

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    SaveSystem Saver = new SaveSystem();

    public string SaveFile, PlayerLocation, HeldItem;
    public float Fire;
    public int FireStage, BirdTrust;
    public List<ItemSO> Inventory;
    [SerializeReference]
    List<ItemSO> reference;
    public List<string> triggers;

	public void Clear()
	{
        PlayerLocation = SceneNames.ACT1_BEDROOM;
        Fire = 0;
        FireStage = 0;
        BirdTrust = 0;
        Inventory = new List<ItemSO>();
        triggers = new List<string>();
        HeldItem = string.Empty;
	}

    public void NewGame(string FileName)
    {
		Clear();
		SaveFile = FileName;
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
        FireStage = data.FireStage;
        BirdTrust = data.BirdTrust;
        Inventory = GetInventoryList(data.Inventory);
        triggers = data.triggers;
    }

    public void AddToInventory(ItemSO item)
    {
        Debug.Log($"adding {item.name} to inventory");
        Inventory.Add(item);
    }

    public bool ItemContains(string name)
    {
        foreach (ItemSO i in Inventory)
        {
            if (i.name == name)
            {
                return true;
            }
        }

        return false;
    }

	public void RemoveItem(string name)
    {
		for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].name == name)
            {
				Inventory.RemoveAt(i);
            }
        }
    }

    public ItemSO GetItem(string name)
    {
        foreach (ItemSO i in Inventory)
        {
            if (i.name == name)
            {
                return i;
            }
        }

        return null;
    }

    PlayerData SOToData()
    {
        PlayerData info = new();

        info.SaveFile = SaveFile;
        info.PlayerLocation = PlayerLocation;
        info.HeldItem = HeldItem;
        info.Fire = Fire;
        info.FireStage = FireStage;
        info.BirdTrust = BirdTrust;
        info.Inventory = SerialiseInven(Inventory);
        info.triggers = triggers;

        return info;
    }

    List<string> SerialiseInven(List<ItemSO> items)
    {
        List<string> list = new List<string>();

        foreach (ItemSO i in items)
        {
            list.Add(i.name);
        }

        return list;
    }

    List<ItemSO> GetInventoryList(List<string> list)
    {
        List<ItemSO> items = new List<ItemSO>();

        foreach (string s in list)
        {
            items.Add(referenceGet(s));
        }

        return items;
    }

    ItemSO referenceGet(string s)
    {
        ItemSO reff = null;

        foreach(ItemSO r in reference)
        {
            if(r.name == s)
            {
                reff = r;
            }
        }

        if (reff != null)
        {
            return reff;
        }
        else
        {
            return null;
        }

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
