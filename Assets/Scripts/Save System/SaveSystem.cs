using System;
using System.IO;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using UnityEngine;

// Gaurav Singh

// This is the script that saves and loads the player data
// it reads and writes our player data by turning them into JSONs and saving them to the persistent data path
// On Windows this path should be the appdata/locallow folder
// On brower this should save to the website somewhere
// it can also check to see if a Save exists there already or not

public class SaveSystem : IDataService
{
    public bool SaveData<T>(string rPath, T data)
    {
        string path = Application.persistentDataPath + rPath;

        if (File.Exists(path))
        {
            Debug.Log("Save Exists, Attempting Rewrite");

            try
            {
                File.Delete(path);
                using FileStream stream = File.Create(path);
                stream.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data));

                Debug.Log("Rewrite Successful");
                Debug.Log(path);
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Rewrite Unsuccessful, reason:  + {e.Message} + {e.StackTrace}");
                return false;
            }
        }
        else
        {
            Debug.Log("New Save, Attempting Write");

            try
            {
                using FileStream stream = File.Create(path);
                stream.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data));

                Debug.Log("Write Successful");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Write Unsuccessful, reason:  + {e.Message} + {e.StackTrace}");
                return false;
            }
        }
    }

    public T LoadData<T>(string rPath)
    {
        string path = Application.persistentDataPath + rPath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. No file found");
            throw new FileNotFoundException();
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            Debug.Log("Load Data Successful");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load Data Unsuccessful, reason:  + {e.Message} + {e.StackTrace}");
            throw e;
        }
        
    }

    public void DeleteData<T>(string rPath)
    {
        string path = Application.persistentDataPath + rPath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot delete file at {path}. No file found");
            throw new FileNotFoundException();
        }

        try
        {
            File.Delete(path);
            Debug.Log("Data Deleted Successful");
        }
        catch (Exception e)
        {
            Debug.LogError($"Delete Data Unsuccessful, reason:  + {e.Message} + {e.StackTrace}");
            throw e;
        }
    }

    public bool CheckData<T>(string rPath)
    {
        string path = Application.persistentDataPath + rPath;

        if (File.Exists(path))
        {
            Debug.Log("true");

            return true;
        }
        else
        {
            Debug.Log("false");

            return false;
        }
    }
}
