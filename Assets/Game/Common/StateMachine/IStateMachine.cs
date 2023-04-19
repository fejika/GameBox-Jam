using System;

namespace Game.Common {
    internal interface IStateMachine {
        event Action<IState> OnStateChanged;
        IState PreviousState { get; }
        IState CurrentState { get; }
        void SwitchState(IState newState);
        void Dispose();
        void PauseStateMachine(bool value);
    }
}