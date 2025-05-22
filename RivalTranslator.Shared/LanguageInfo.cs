namespace RivalTranslator.Shared;
public record LanguageInfo(string Code, string Name)
{
    public override string ToString() => Name;
}
