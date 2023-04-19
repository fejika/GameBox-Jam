using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Common {
    public class GameTime : IGameTime, IUpdateListener, IDisposable {
        public event Action OnSecondTickEvent;
        private float timeSinceGameStarted;
        private float timeSinceSessionStarted;
        private float secondsTimer;
        IMainAppUpdater appUpdater;


        [Inject]
        public GameTime(IMainAppUpdater up) {
            appUpdater = up;
            appUpdater.AddListener(this);
        }

        public void OnUpdate(float unscaledDeltaTime) {
            timeSinceGameStarted += unscaledDeltaTime;
            timeSinceSessionStarted += unscaledDeltaTime;
            CalculateTime(unscaledDeltaTime);
        }

        private void CalculateTime(float unscaledDeltaTime) {
            secondsTimer += unscaledDeltaTime;
            if (!(secondsTimer > 1f)) return;

            secondsTimer -= Mathf.FloorToInt(secondsTimer);
            OnSecondTickEvent?.Invoke();
        }

        public void Dispose() {
            appUpdater.RemoveListener(this);
        }
    }
}