using UnityEngine;

namespace Services
{
    /// <summary>
    /// PlayerPrefs-based implementation of ISaveService.
    /// </summary>
    public sealed class SaveService : ISaveService
    {
        public void Save(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        public string Load(string key)
        {
            return PlayerPrefs.GetString(key, string.Empty);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
