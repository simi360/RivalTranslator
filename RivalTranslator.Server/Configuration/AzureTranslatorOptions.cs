namespace RivalTranslator.Server.Configuration;
public class AzureTranslatorOptions
{
    public string Endpoint { get; set; } = default!;
    public string SubscriptionKey { get; set; } = default!;
    public string Region { get; set; } = default!;
}
