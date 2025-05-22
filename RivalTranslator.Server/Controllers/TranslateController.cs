using Microsoft.AspNetCore.Mvc;
using RivalTranslator.Shared;
using RivalTranslator.Server.Services;

namespace RivalTranslator.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TranslateController : ControllerBase
{
    private readonly ITranslator _translator;
    public TranslateController(ITranslator translator) => _translator = translator;

    [HttpGet("languages")]
    public async Task<IEnumerable<LanguageInfo>> Languages()
        => await _translator.GetSupportedLanguagesAsync();

    [HttpPost]
    public async Task<TranslateResponse> Post([FromBody] TranslateRequest req)
        => new(await _translator.TranslateAsync(req.Text, req.From, req.To));
}