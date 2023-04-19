using Game;
using Game.Common;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller {
    [SerializeField] private MainAppUpdater appUpdater;
    [SerializeField] private AudioSystem audioPrefab;

    private MainApp mainApp;

    public override void InstallBindings() {
        BindMainAppUpdater();
        BindMainApp();
        BindStorage();
        BindGame();
        BindAudio();
    }

    private void BindMainAppUpdater() {
        var instance = Instantiate(appUpdater);
        Container.Bind<IMainAppUpdater>().FromInstance(instance);
    }

    private void BindMainApp() {
        mainApp = new MainApp(appUpdater);
        mainApp.ChangeState(AppState.Initializing);
        Container.Bind<IMainApp>().FromInstance(mainApp).AsSingle().NonLazy();
    }

    private void BindStorage() {
        Container.BindInterfacesAndSelfTo<GameStorage>().AsSingle();
    }

    private void BindGame() {
        Container.BindInterfacesAndSelfTo<GameRepository>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameFacade>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimersFactory>().AsSingle();
    }

    private void BindAudio() {
        var instance = Container.InstantiatePrefabForComponent<IAudioSystem>(audioPrefab);
        Container.Bind<IAudioSystem>().FromInstance(instance);
    }

}