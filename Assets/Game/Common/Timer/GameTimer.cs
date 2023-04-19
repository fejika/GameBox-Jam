using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameEngine {
    public class GameTimer : MonoBehaviour {
        #region EVENTS

        public event Action OnTimerStartedEvent;
        public event Action OnTimerPausedEvent;
        public event Action OnTimerCompletedEvent;
        public event Action OnTimerValueChangedEvent;

        #endregion

        private readonly int _initialValue;

        public int timerValue { get; set; }
        public bool isActive { get; private set; }

        public GameTimer(int timerValue) {
            _initialValue = timerValue;
            this.timerValue = timerValue;
            isActive = false;
        }

        public GameTimer() {
            _initialValue = 0;
            timerValue = 0;
            isActive = false;
        }

        /*public GameTimer(DateTime endDate) {
            var dateDiff = endDate - GameTime.now;
            var secondsDiff = (int)Math.Floor(dateDiff.TotalSeconds);
            var totalSeconds = Math.Max(0, secondsDiff);
            _initialValue = totalSeconds;
            this.timerValue = totalSeconds;
            isActive = false;
        }*/

        public void Start() {
            if (isActive)
                return;

            isActive = true;
            OnTimerStartedEvent?.Invoke();

            if (timerValue == 0) {
                Stop();
                return;
            }
        }

        private void OnAppUnpaused() {
            RecalculateTimer();
        }

        private void RecalculateTimer() {
            //var offlineSeconds = (int)Math.Floor(GameTime.timeSinceLastSession);
            //timerValue = Math.Max(this.timerValue - offlineSeconds, 0);
            OnTimerValueChangedEvent?.Invoke();

            if (timerValue == 0)
                Stop();
        }

        public void Pause() {
            if (!isActive)
                return;

            isActive = false;
            OnTimerPausedEvent?.Invoke();
        }

        public void Stop() {
            if (!isActive)
                return;
            isActive = false;
            timerValue = 0;
            OnTimerCompletedEvent?.Invoke();
        }

        public void Reset() {
            timerValue = _initialValue;
            isActive = false;
        }

        #region EVENTS

        private void OnGameTimeSecondTick() {
            timerValue = Math.Max(this.timerValue - 1, 0);
            OnTimerValueChangedEvent?.Invoke();

            if (timerValue == 0)
                Stop();
        }

        #endregion
    }
}