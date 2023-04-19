using UnityEngine;

namespace Game.UI {
	internal sealed class UILayer : UIElement {

		[SerializeField] private UILayerType _layer;

		public UILayerType layer => _layer;

	}
}