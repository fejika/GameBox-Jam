using System;
using Zenject;

namespace Game.Common {
    internal class StateMachine : IUpdateListener, IFixedUpdateListener, IDisposable, IStateMachine {
        public event Action<IState> OnStateChanged;
        
        public IState PreviousState { get; private set; }
        public IState CurrentState { get; private set; }

        private IMainAppUpdater appUpdater;
        private bool isPause = false;

        [Inject]
        public StateMachine(IMainAppUpdater updater) {
            appUpdater = updater;
            appUpdater.AddListener(this);
        }

        public void SwitchState(IState newState) {
            CurrentState?.OnStateExit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.OnStateEnter();
            
            OnStateChanged?.Invoke(newState);
        }

        public void OnUpdate(float deltaTime) {
            if (isPause) return;
            if (CurrentState is IUpdatableState updatableState) {
                updatableState.OnFrameUpdate(deltaTime);
            }
        }
        
        public void OnFixedUpdate(float fixedDeltaTime) {
            if (isPause) return;
            if (CurrentState is IUpdatableState updatableState) {
                updatableState.OnFixedFrameUpdate(fixedDeltaTime);
            }
        }

        public void Dispose() {
            appUpdater.RemoveListener(this);
            CurrentState.Dispose();
        }

        public void PauseStateMachine(bool value) {
            isPause = value;
        }
    }
}