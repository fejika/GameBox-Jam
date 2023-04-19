using UnityEngine;
using Zenject;

namespace Game.UI
{
    internal abstract class UIScreen : UIElement
    {
        [SerializeField] protected UILayerType _layer;
        [SerializeField] protected bool _showByDefault;
        protected IAudioSystem audioSystem;

        [Inject]
        private void Construct(IAudioSystem audio) {
            audioSystem = audio;
        }
        public UILayerType layer => _layer;
        
        public bool showByDefault => _showByDefault;
    }
}