namespace TowerOfBlocks.MVP.Interfaces
{
    /// <summary>
    /// View contract for displaying short messages.
    /// </summary>
    public interface IMessageView
    {
        void ShowMessage(string text);
        void HideMessage();
    }
}

