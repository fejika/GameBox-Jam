namespace Game.UI
{
    internal interface IUIScreen : IUIElement
    {
        bool showByDefault { get; }
    }
}