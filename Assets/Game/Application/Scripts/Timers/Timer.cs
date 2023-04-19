using System;

namespace Game.Common {
    internal class Timer : ITimer, IUpdateListener {
        public event Action OnCompleted;
        
        private float curTime, targetTime, startingTime;
        private IMainAppUpdater appUpdater;
        private bool isPaused, isWorking;

        public bool IsWorking => isWorking;
        
        public Timer(float seconds, IMainAppUpdater updater) {
            curTime = 0f;
            startingTime = seconds;
            targetTime = seconds;
            appUpdater = updater;
        }
        
        public void SetTime(float seconds) {
            startingTime = seconds;
            targetTime = seconds;
        }

        public void Start() {
            if(isWorking) return;
            
            isPaused = false;
            isWorking = true;
            appUpdater.AddListener(this);
        }

        public void Pause() {
            isPaused = true;
        }

        public void OnUpdate(float deltaTime) {
            if(CantCalculateTime()) return;

            CalculateTime(deltaTime);
        }

        private bool CantCalculateTime() {
            return isPaused || !isWorking;
        }

        private void CalculateTime(float deltaTime) {
            curTime += deltaTime;
            
            if (curTime < targetTime) return;

            Stop();
            OnCompleted?.Invoke();
            Reset();
        }
        
        public void Stop() {
            isWorking = false;
            appUpdater.RemoveListener(this);
        }

        public void Reset() {
            curTime = 0f;
            targetTime = startingTime;
        }

        public void Dispose() {
            if(isWorking) Stop();
        }
    }
}