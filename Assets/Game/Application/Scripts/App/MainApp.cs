using System;
using UnityEngine;
using Zenject;

namespace Game {
    public sealed class MainApp : IMainApp {
        public AppState State { get; private set; }
        private MainAppUpdater updater;
        [Inject]
        public MainApp(MainAppUpdater updater) {
            this.updater = updater;
            UnityEngine.Application.targetFrameRate = 60;
        }       
        public void ChangeState(AppState newState) {
            State = newState;
        }
        public void SetPause(bool value) {
            updater.SetPause(value);
            Time.timeScale = value ? 0f : 1f;
        }
    }
}