namespace ReferenceController;

[ApiController]
[Route(nameof(Gemini))]
public class Gemini(IGemini GeminiService) : Controller
{
    private readonly IGemini _GeminiService = GeminiService;

    // [Authorize]
    [HttpGet]
    public async Task<IActionResult> Chat([FromQuery] string input)
    {
        var result = await _GeminiService.Chat(input);
        return Ok(result);
    }
}
