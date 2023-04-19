namespace Game {
    public interface IUpdateListener : IAppUpdateListener {
        void OnUpdate(float deltaTime);
    }
}