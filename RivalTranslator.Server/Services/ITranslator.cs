using RivalTranslator.Shared;

namespace RivalTranslator.Server.Services;

public interface ITranslator
{
    Task<string> TranslateAsync(string text, string from, string to);
    Task<IEnumerable<LanguageInfo>> GetSupportedLanguagesAsync();
}