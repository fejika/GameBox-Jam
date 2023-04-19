﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 using Zenject;

 namespace Game.UI
{
    //TODO: Divide for 3 instance:  UIScreenManager, UIPopupManager, UIControllerManager
    internal interface IUIController {
        event Action OnUIBuilt;
        T ShowUIElement<T>() where T : UIElement, IUIElement;
        T CreateAndShowElement<T>(IUIElement prefab, UILayerType layerType) where T : UIElement, IUIElement;
        void BuildUI(DiContainer container);
        IUIElement[] GetAllCreatedUIElements();
        T GetUIElement<T>() where T : UIElement;
        bool IsAnyPopupOpened(out UIPopup openedPopup);
    }

    internal class UIController : MonoBehaviour, IUIController {
        public event Action OnUIBuilt;
        
        [SerializeField]
        private UIConfig config;

        [SerializeField]
        private Camera _uiCamera;

        [SerializeField]
        private Camera _uiTutorialCamera;

        [SerializeField]
        private EventSystem _eventSystem;

        [SerializeField]
        private GraphicRaycaster _graphicRaycaster;

        [SerializeField]
        private UILayer[] _layers;

        public Camera uiCamera => _uiCamera;
        public Camera uiTutorialCamera => _uiTutorialCamera;
        
        public bool isUIBuilt { get; private set; }
        public bool isLoggingEnabled { get; set; }
        private DiContainer diContainer;

        private Dictionary<Type, IUIElement> createdUIElementsMap;
        private Dictionary<Type, UIPopup> cachedPopupsMap;

        private Dictionary<Type, RememberedElement> rememberedElementMap;


        private void Awake()
        {
            createdUIElementsMap = new Dictionary<Type, IUIElement>();
            cachedPopupsMap = new Dictionary<Type, UIPopup>();
            rememberedElementMap = new Dictionary<Type, RememberedElement>();

            _uiCamera = Camera.main;
            //DontDestroyOnLoad(gameObject);
        }

        #region SHOW

        public T ShowUIElement<T>() where T : UIElement, IUIElement
        {
            var type = typeof(T);

            if (createdUIElementsMap.TryGetValue(type, out var foundElement) && foundElement.isActive)
                return (T) foundElement;

            cachedPopupsMap.TryGetValue(type, out var cachedPopup);
            if (cachedPopup != null)
            {
                cachedPopup.Show();
                return cachedPopup as T;
            }

            if (this.rememberedElementMap.TryGetValue(type, out var element))
            {
                return this.CreateAndShowElement<T>(element.prefab, element.layerType);
            }

            var prefab = config.FindPrefab(type, out var layerType);
            return CreateAndShowElement<T>(prefab, layerType);
        }

        public T CreateAndShowElement<T>(IUIElement prefab, UILayerType layerType) where T : UIElement, IUIElement
        {
            var container = GetContainer(layerType);
            var createdElementGo = diContainer.InstantiatePrefab(prefab.gameObject, container);
            createdElementGo.name = prefab.name;
            var createdElement = createdElementGo.GetComponent<T>();
            var type = typeof(T);

            createdUIElementsMap[type] = createdElement;
            createdElement.Show();
            createdElement.OnElementHiddenCompletelyEvent += OnElementHiddenCompletely;
            return createdElement;
        }

        private void OnElementHiddenCompletely(IUIElement uiElement)
        {
            if (uiElement is IUIPopup uiPopup && uiPopup.isPreCached)
                return;

            var type = uiElement.GetType();
            uiElement.OnElementHiddenCompletelyEvent -= OnElementHiddenCompletely;
            createdUIElementsMap.Remove(type);
        }

        #endregion


        #region BUILD

        public void BuildUI(DiContainer diContainer) {
            this.diContainer = diContainer;
            this.BuildScreens();
            this.BuildPopups();
            this.BuildTutorial();
            this.BuildControllers();

            isUIBuilt = true;
            OnUIBuilt?.Invoke();
            Resources.UnloadUnusedAssets();
        }

        private void BuildScreens()
        {
            var prefabs = this.config.GetPrefabs(UILayerType.Screen);
            for (int i = 0, count = prefabs.Length; i < count; i++)
            {
                var prefab = (UIScreen) prefabs[i];
                if (prefab.showByDefault)
                {
                    this.CreateAndShowScreen(prefab);
                }
                else
                {
                    this.RememberTypeForLaterCreation(prefab, UILayerType.Screen);
                }
            }
        }

        private void BuildPopups()
        {
            var prefabs = this.config.GetPrefabs(UILayerType.Popup);
            for (int i = 0, count = prefabs.Length; i < count; i++)
            {
                var prefab = (UIPopup) prefabs[i];
                if (prefab.isPreCached)
                {
                    this.CreateCachedPopup(prefab, UILayerType.Popup);
                }
                else
                {
                    this.RememberTypeForLaterCreation(prefab, UILayerType.Popup);
                }
            }
        }

        private void BuildTutorial() {
            var prefabs = this.config.GetPrefabs(UILayerType.Tutorial);
            for (int i = 0, count = prefabs.Length; i < count; i++)
            {
                var prefab = (UIPopup) prefabs[i];
                if (prefab.isPreCached)
                {
                    this.CreateCachedPopup(prefab, UILayerType.Tutorial);
                }
                else
                {
                    this.RememberTypeForLaterCreation(prefab, UILayerType.Tutorial);
                }
            }
        }

        private void CreateCachedPopup(UIPopup popupPref, UILayerType layerType)
        {
            var container = GetContainer(layerType);
            var createdCachedPopup = diContainer.InstantiatePrefabForComponent<UIPopup>(popupPref, container);
            createdCachedPopup.name = popupPref.name;
            var type = createdCachedPopup.GetType();

            cachedPopupsMap[type] = createdCachedPopup;
            createdUIElementsMap[type] = createdCachedPopup;

            createdCachedPopup.HideInstantly();
        }
        
        private void BuildControllers()
        {
            var container = GetContainer(UILayerType.Controller);
            var prefabs = this.config.GetPrefabs(UILayerType.Controller);
            for (int i = 0, count = prefabs.Length; i < count; i++)
            {
                var prefab = prefabs[i];
                this.CreateController(prefab, container);
            }
        }

        private void CreateController(IUIElement prefab, Transform container)
        {
            var createdElementGo = diContainer.InstantiatePrefab(prefab.gameObject, container);;
            createdElementGo.name = prefab.name;
            var createdElement = createdElementGo.GetComponent<IUIElement>();
            var type = createdElement.GetType();

            createdElement.uiController = this;//TODO
            createdElement.gameObject.SetActive(true);//TODO

            createdUIElementsMap[type] = createdElement;
            createdElement.Show();
            createdElement.OnElementHiddenCompletelyEvent += OnElementHiddenCompletely;
        }

        private void CreateAndShowScreen(UIScreen uiScreenPref)
        {
            var container = GetContainer(uiScreenPref.layer);
            var createdUIScreen = diContainer.InstantiatePrefabForComponent<UIScreen>(uiScreenPref, container);
            createdUIScreen.name = uiScreenPref.name;
            var type = createdUIScreen.GetType();
            createdUIElementsMap[type] = createdUIScreen;
            createdUIScreen.Show();
        }

        private Transform GetContainer(UILayerType layer)
        {
            return _layers.First(layerObject => layerObject.layer == layer).transform;
        }

        private void RememberTypeForLaterCreation(IUIElement prefab, UILayerType layerType)
        {
            var rememberedElement = new RememberedElement(prefab, layerType);
            var type = prefab.GetType();
            this.rememberedElementMap.Add(type, rememberedElement);
        }

        #endregion


        public IUIElement[] GetAllCreatedUIElements()
        {
            return createdUIElementsMap.Values.ToArray();
        }

        public T GetUIElement<T>() where T : UIElement
        {
            var type = typeof(T);
            createdUIElementsMap.TryGetValue(type, out var uiElement);
            return (T) uiElement;
        }

        public void Clear()
        {
            if (createdUIElementsMap == null)
                return;

            var allCreatedUIElements = createdUIElementsMap.Values.ToArray();
            foreach (var uiElement in allCreatedUIElements)
                Destroy(uiElement.gameObject);

            createdUIElementsMap.Clear();
            cachedPopupsMap.Clear();
            rememberedElementMap.Clear();
        }

        public bool IsAnyPopupOpened(out UIPopup openedPopup) {
            foreach (var elPair in createdUIElementsMap) {
                if(!(elPair.Value is UIPopup popup) || !popup.isActive) continue;

                openedPopup = popup;
                return true;
            }

            openedPopup = null;
            return false;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_uiCamera == null)
                _uiCamera = GetComponentInChildren<Camera>();

            _layers = GetComponentsInChildren<UILayer>();
        }
#endif
        
        public List<RaycastResult> Raycast(Vector2 screenPosition)
        {
            var pointerEventData = new PointerEventData(this._eventSystem)
            {
                position = screenPosition
            };

            var raycastResults = new List<RaycastResult>();
            this._graphicRaycaster.Raycast(pointerEventData, raycastResults);
            return raycastResults;
        }

        public bool IsPointerOverUI()
        {
            return this._eventSystem.IsPointerOverGameObject();
        }

        public bool IsPointerOverUI(int fingerId)
        {
            return this._eventSystem.IsPointerOverGameObject(fingerId);
        }

        private readonly struct RememberedElement
        {
            public readonly IUIElement prefab;

            public readonly UILayerType layerType;

            public RememberedElement(IUIElement prefab, UILayerType layerType)
            {
                this.prefab = prefab;
                this.layerType = layerType;
            }
        }

        public UILayer GetLayer(UILayerType layerType) {
            return _layers.First(layer => layer.layer == layerType);
        }
    }
}