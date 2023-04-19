﻿using System;
using UnityEngine;

namespace Game.UI {
	internal interface IUIElement {

		#region EVENTS

		event Action<IUIElement> OnElementHideStartedEvent;
		event Action<IUIElement> OnElementHiddenCompletelyEvent;
		event Action<IUIElement> OnElementShownEvent;
		event Action<IUIElement> OnElementDestroyedEvent;

		#endregion

		bool isActive { get; }
		string name { get; }
		GameObject gameObject { get; }

		public UIController uiController { get; set; }//TODO

		void Show();
		void Hide();
		void HideInstantly();

	}
}