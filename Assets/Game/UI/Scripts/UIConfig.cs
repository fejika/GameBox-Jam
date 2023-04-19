using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "UI/New UIConfig")]
    internal sealed class UIConfig : ScriptableObject, ISerializationCallbackReceiver {
        [Header("Screen Prefabs")]
        [SerializeField]
        private Layer screenLayer = new Layer {
            type = UILayerType.Screen,
            prefabs = new GameObject[0]
        };

        [Header("Popup Prefabs")]
        [SerializeField]
        private Layer popupLayer = new Layer {
            type = UILayerType.Popup,
            prefabs = new GameObject[0]
        };
        
        [Header("Tutorial Prefabs")]
        [SerializeField]
        private Layer tutorialLayer = new Layer {
            type = UILayerType.Tutorial,
            prefabs = new GameObject[0]
        };
        
        [Header("Controller Prefabs")]
        [SerializeField]
        private Layer controllerLayer = new Layer {
            type = UILayerType.Controller,
            prefabs = new GameObject[0]
        };

        private Dictionary<UILayerType, GameObject[]> layerTable;

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            layerTable = new Dictionary<UILayerType, GameObject[]>
            {
                {screenLayer.type, screenLayer.prefabs},
                {popupLayer.type, popupLayer.prefabs},
                {tutorialLayer.type, tutorialLayer.prefabs},
                {controllerLayer.type, controllerLayer.prefabs}
            };
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
        }

        public IUIElement[] GetPrefabs(UILayerType layerType) {
            var prefabs = layerTable[layerType];
            var count = prefabs.Length;
            var result = new IUIElement[count];
            for (var i = 0; i < count; i++)
            {
                var prefab = prefabs[i];
                result[i] = prefab.GetComponent<IUIElement>();
            }
            
            return result;
        }

        public IUIElement FindPrefab(Type type, out UILayerType layerType) {
            if (TryFindPrefab(popupLayer, type, out var popup))
            {
                layerType = UILayerType.Popup;
                return popup;
            }
            
            if (TryFindPrefab(tutorialLayer, type, out var tutorial))
            {
                layerType = UILayerType.Tutorial;
                return tutorial;
            }

            if (TryFindPrefab(controllerLayer, type, out var controller))
            {
                layerType = UILayerType.Controller;
                return controller;
            }

            if (TryFindPrefab(screenLayer, type, out var screen))
            {
                layerType = UILayerType.Screen;
                return screen;
            }

            throw new Exception($"Element of type {type.Name} is not found!");
        }

        private bool TryFindPrefab(Layer layer, Type type, out IUIElement result)
        {
            var prefabs = layer.prefabs;
            for (int i = 0, count = prefabs.Length; i < count; i++)
            {
                var prefab = prefabs[i];
                var element = prefab.GetComponent<IUIElement>();
                if (element.GetType() == type)
                {
                    result = element;
                    return true;
                }
            }

            result = null;
            return false;
        }

        [Serializable]
        private struct Layer
        {
            [SerializeField]
            public UILayerType type;

            [SerializeField]
            public GameObject[] prefabs;
        }
    }
}