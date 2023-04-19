namespace Game {
    public interface IFixedUpdateListener : IAppUpdateListener {
        void OnFixedUpdate(float fixedDeltaTime);
    }
}