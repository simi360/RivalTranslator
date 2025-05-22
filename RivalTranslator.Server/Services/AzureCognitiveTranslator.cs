using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RivalTranslator.Server.Configuration;
using RivalTranslator.Shared;

namespace RivalTranslator.Server.Services;
public class AzureCognitiveTranslator : ITranslator
{
    private readonly HttpClient _http;
    private readonly AzureTranslatorOptions _opts;
    public AzureCognitiveTranslator(HttpClient http, IOptions<AzureTranslatorOptions> opts)
    {
        _http = http;
        _opts = opts.Value;
    }

    public async Task<string> TranslateAsync(string text, string from, string to)
    {
        var endpoint = _opts.Endpoint.TrimEnd('/');
        var url = $"{endpoint}/translate?api-version=3.0&from={from}&to={to}";

        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Add("Ocp-Apim-Subscription-Key", _opts.SubscriptionKey);
        req.Headers.Add("Ocp-Apim-Subscription-Region", _opts.Region);

        req.Content = new StringContent(
          JsonSerializer.Serialize(new[] { new { Text = text } }),
          Encoding.UTF8, "application/json");

        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
        return doc.RootElement[0]
                  .GetProperty("translations")[0]
                  .GetProperty("text")
                  .GetString()!;
    }

    public async Task<IEnumerable<LanguageInfo>> GetSupportedLanguagesAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_opts.Endpoint}languages?api-version=3.0");

            request.Headers.Add("Ocp-Apim-Subscription-Key", _opts.SubscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", _opts.Region);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement.GetProperty("translation");

            var result = new List<LanguageInfo>();
            foreach (var prop in root.EnumerateObject())
            {
                var code = prop.Name;
                var name = prop.Value.GetProperty("name").GetString()!;
                result.Add(new LanguageInfo(code, name));
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Azure API call failed: " + ex.Message);
            return new List<LanguageInfo>();
        }
    }


}
