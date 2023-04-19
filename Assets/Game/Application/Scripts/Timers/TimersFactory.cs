using System;
using System.Collections.Generic;

namespace Game.Common {
    internal class TimersFactory : IDisposable {
        private IMainAppUpdater appUpdater;
        private List<IDisposable> createdTimers;

        public TimersFactory(IMainAppUpdater appUpdater) {
            this.appUpdater = appUpdater;
            createdTimers = new List<IDisposable>();
        }

        public ITimer Create(float seconds = 0) {
            var timer = new Timer(seconds, appUpdater);
            createdTimers.Add(timer);
            return timer;
        }
        
        public void Dispose() {
            foreach (var timer in createdTimers) {
                timer.Dispose();
            }
            createdTimers.Clear();
        }
    }
}