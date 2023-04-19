using System;
using UnityEngine;

namespace Game {
    public class GameLoader : MonoBehaviour {
        [SerializeField] string firstLoadScene;
        private void Start() {
            using var sceneLoader = new SceneLoader();
            sceneLoader.LoadScene(firstLoadScene);
        }
    }
}