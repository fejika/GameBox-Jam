namespace Game.Common {
    public interface IGameStorage {
        void Save<T>(string prefKey, T data);
        T Load<T>(string prefKey, T defaultValue = default);
        bool HasKey(string prefKey);
        void ClearData(string key);
    }
}