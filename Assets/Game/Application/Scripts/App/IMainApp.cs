namespace Game {
    public interface IMainApp {
        AppState State { get; }
        void ChangeState(AppState newState);
        void SetPause(bool value);
    }
}