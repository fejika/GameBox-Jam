using System;
using Game.Common;

namespace Game {
    internal sealed class GameRepository : Repository {
        protected override string StorageKey => "GameData";
        protected override object DefaultValue => new GameData();

        public GameRepository(IGameStorage storage) : base(storage) { }
    }

    [Serializable]
    internal struct GameData {
        
    }
}