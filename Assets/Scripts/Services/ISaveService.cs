namespace Services
{
    public interface ISaveService
    {
        void Save(string key, string data);
        string Load(string key);
        bool HasKey(string key);
        void DeleteKey(string key);
    }
}
