// Gaurav Singh

// this is a custom class type for the Save System to utalise, it creates the methods for that class
public interface IDataService
{
    bool SaveData<T> (string rPath, T data);

    T LoadData<T>(string rPath);

    public bool CheckData<T>(string rPath);
}
