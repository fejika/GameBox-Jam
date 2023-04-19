namespace Game.Common {
    internal abstract class Repository {
        protected abstract string StorageKey {get;}
        protected abstract object DefaultValue { get; }
        
        protected IGameStorage storage;

        protected Repository(IGameStorage storage) {
            this.storage = storage;
        }
        
        public virtual void Save<T>(T data) {
            storage.Save(StorageKey, data);
        }

        public virtual T Load<T>() {
            return storage.Load(StorageKey, GetDefaultValue<T>());
        }

        protected T GetDefaultValue<T>() {
            return (T) DefaultValue;
        }
    }
}