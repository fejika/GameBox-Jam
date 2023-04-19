using Game.UI;
using UnityEngine;
using Zenject;

namespace Game {
    public class MainGameInstaller : MonoInstaller {
        [SerializeField] private UIController uiController;
        private UIController uiControllerInstance;
        public override void InstallBindings() {
            BindUIController();
            BuildUi();
        }
        private void BindUIController() {
            uiControllerInstance = Container.InstantiatePrefabForComponent<UIController>(uiController);
            Container.Bind<IUIController>().FromInstance(uiControllerInstance);
        }

        private void BuildUi() {
            uiControllerInstance.BuildUI(Container);
        }
    }
}