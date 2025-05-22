using Microsoft.AspNetCore.Mvc;
using RivalTranslator.Shared;

namespace RivalTranslator.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TranslateController : ControllerBase
{
    private readonly ITranslator _translator;
    public TranslateController(ITranslator translator) => _translator = translator;

    [HttpGet("languages")]
    public Task<IEnumerable<LanguageInfo>> Languages()
    {
        return Task.FromResult<IEnumerable<LanguageInfo>>(new[]
        {
        new LanguageInfo("en", "English"),
        new LanguageInfo("fr", "French"),
        new LanguageInfo("es", "Spanish")
    });
    }

    [HttpPost]
    public async Task<TranslateResponse> Post([FromBody] TranslateRequest req)
      => new(await _translator.TranslateAsync(req.Text, req.From, req.To));
}
