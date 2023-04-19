using System;

namespace Game.Common {
    internal interface IState {
        event Action OnStateEnded;
        void OnStateExit();
        void OnStateEnter();
        IState Clone();
        void Dispose();
    }
}