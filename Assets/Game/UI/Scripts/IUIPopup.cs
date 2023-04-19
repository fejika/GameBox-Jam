using System;

namespace Game.UI
{
    internal interface IUIPopup : IUIElement
    {
        event Action<IUIPopup> OnPopupHiddenCompletelyEvent;
        
        bool isPreCached { get; }
    }
}