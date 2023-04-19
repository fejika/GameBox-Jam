namespace Game.Common {
    internal interface IUpdatableState : IState {
        void OnFixedFrameUpdate(float deltaTime);
        void OnFrameUpdate(float deltaTime);
    }
}