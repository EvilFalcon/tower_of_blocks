namespace Services
{
    /// <summary>
    /// Service for text localization.
    /// </summary>
    public interface ILocalizationService
    {
        string GetText(string key);
    }
}
