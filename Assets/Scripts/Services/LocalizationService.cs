namespace Services
{
    // TODO: For real localization, strings need to be substituted from tables by key instead of returning key (e.g., via ScriptableObject or JSON)
    public sealed class LocalizationService : ILocalizationService
    {
        public string GetText(string key)
        {
            return key;
        }
    }
}
