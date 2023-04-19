using Zenject;

namespace Game {
    internal class GameFacade : IInitializable {
        private readonly GameRepository repository;
        private GameData data;

        [Inject]
        public GameFacade(GameRepository repository) {
            this.repository = repository;
        }
        public void Initialize() {
        }
        private void SaveData() {
            repository.Save(data);
        }
    }
}