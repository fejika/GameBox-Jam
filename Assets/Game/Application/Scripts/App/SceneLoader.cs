using System;
using Unity;
using UnityEngine.SceneManagement;

namespace Game {
    public interface ISceneLoader {
        void LoadScene(string name, bool additive = false);
    }

    public class SceneLoader : ISceneLoader, IDisposable {
        public void LoadScene(string name, bool additive = false) {
            var loadMethod = (additive) ? LoadSceneMode.Additive : LoadSceneMode.Single;
            SceneManager.LoadScene(name, loadMethod);
        }

        public void Dispose() {
            
        }
    }
}