using UnityEngine;

namespace Game.Common {
    public class GameStorage : IGameStorage {
        public void Save<T>(string prefKey, T data) {
            var jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(prefKey, jsonData);
            PlayerPrefs.Save();
        }

        public T Load<T>(string prefKey, T defaultValue = default) {
            if (!HasKey(prefKey)) return defaultValue;
            
            var jsonData = PlayerPrefs.GetString(prefKey);
            var loadedData = JsonUtility.FromJson<T>(jsonData);
            return loadedData;
        }
        
        public void ClearData(string key) {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public bool HasKey(string prefKey) {
            return PlayerPrefs.HasKey(prefKey);
        }
    }
}