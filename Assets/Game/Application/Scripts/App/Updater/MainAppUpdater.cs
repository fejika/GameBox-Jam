using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public sealed class MainAppUpdater : MonoBehaviour, IMainAppUpdater {
        private float fixedDeltaTime;
        private List<IUpdateListener> updateListeners;
        private List<IFixedUpdateListener> fixedUpdateListeners;
        private bool isPaused;
        
        public void SetPause(bool value) {
            isPaused = value;
        }
        public void AddListener(IAppUpdateListener listener) {
            if(listener is IUpdateListener updateListener) updateListeners.Add(updateListener);
            if(listener is IFixedUpdateListener fixedUpdateListener) fixedUpdateListeners.Add(fixedUpdateListener);
        }

        public void RemoveListener(IAppUpdateListener listener) {
            if(listener is IUpdateListener updateListener) updateListeners.Remove(updateListener);
            if(listener is IFixedUpdateListener fixedUpdateListener) fixedUpdateListeners.Remove(fixedUpdateListener);
        }
        
        private void Awake() {
            fixedDeltaTime = Time.fixedDeltaTime;
            updateListeners = new List<IUpdateListener>();
            fixedUpdateListeners = new List<IFixedUpdateListener>();
            DontDestroyOnLoad(this);
        }
        
        private void FixedUpdate() {
            if(isPaused) return;
            
            for (int i = 0, count = fixedUpdateListeners.Count; i < count; i++) {
                var listener = fixedUpdateListeners[i];
                listener.OnFixedUpdate(fixedDeltaTime);
            }
        }

        private void Update() {
            if(isPaused) return;
            
            var deltaTime = Time.unscaledDeltaTime;
            
            for (int i = 0; i < updateListeners.Count; i++) {
                var listener = updateListeners[i];
                listener.OnUpdate(deltaTime);
            }
        }
    }
}