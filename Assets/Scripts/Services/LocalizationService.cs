namespace Services
{
    /// <summary>
    /// Stub implementation of ILocalizationService.
    /// Returns key as-is for now; can be extended later.
    /// </summary>
    public sealed class LocalizationService : ILocalizationService
    {
        public string GetText(string key)
        {
            return key;
        }
    }
}
