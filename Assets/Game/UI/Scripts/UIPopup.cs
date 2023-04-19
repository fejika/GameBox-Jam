﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    internal abstract class UIPopup : UISceneElement, IUIPopup
    {
        public event Action<IUIPopup> OnPopupHiddenCompletelyEvent;
        protected IAudioSystem audioSystem;
        
        [SerializeField]
        protected bool _isPreCached;

        public bool isPreCached => _isPreCached;

        private Canvas canvas { get; set; }

        #region AWAKE AND INITIALIZATION
        [Inject]
        private void Construct(IAudioSystem audio) {
            audioSystem = audio;
        }
        protected override void OnAwake() {
            base.OnAwake();
            
            if (isPreCached)
                InitPreCachedPopup();
        }

        private void InitPreCachedPopup()
        {
            InitCanvas();
            InitRaycaster();
        }

        private void InitCanvas()
        {
            canvas = gameObject.GetComponent<Canvas>();
            if (!canvas)
                canvas = gameObject.AddComponent<Canvas>();
        }

        private void InitRaycaster()
        {
            var raycaster = gameObject.GetComponent<GraphicRaycaster>();
            if (!raycaster)
                gameObject.AddComponent<GraphicRaycaster>();
        }

        #endregion


        #region SHOW

        public sealed override void Show()
        {
            if (isActive)
                return;

            OnPreShow();

            if (isPreCached)
            {
                transform.SetAsLastSibling();
                canvas.enabled = true;
            }

            isActive = true;
            gameObject.SetActive(true);
            OnPostShow();
            NotifyAboutShown();
        }
        
        #endregion


        #region HIDE

        public sealed override void HideInstantly()
        {
            if (!isActive)
                return;

            if (isPreCached)
            {
                canvas.enabled = false;
                gameObject.SetActive(false);
            }
            else
                Destroy(gameObject);

            isActive = false;
            OnPopupHiddenCompletelyEvent?.Invoke(this);
            OnPostHide();
        }

        #endregion


        #region EVENTS

        private void OnCloseButtonClick()
        {
            Hide();
        }

        #endregion
    }
}