namespace Game {
    public interface IMainAppUpdater {
        void SetPause(bool value);
        void AddListener(IAppUpdateListener listener);
        void RemoveListener(IAppUpdateListener listener);
    }
}