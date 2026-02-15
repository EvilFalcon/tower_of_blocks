namespace Services
{
    public interface IGameStateLoader
    {
        bool HasSavedState();
        void LoadState();
    }
}
