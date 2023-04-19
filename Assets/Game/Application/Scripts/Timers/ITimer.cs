using System;

namespace Game.Common {
    internal interface ITimer : IDisposable {
        event Action OnCompleted;
        void SetTime(float seconds);
        void Start();
        void Pause();
        void Stop();
        void Reset();
        bool IsWorking { get;}
    }
}