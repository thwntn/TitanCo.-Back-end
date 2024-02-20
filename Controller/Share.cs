namespace ReferenceController;

[ApiController]
[Route(nameof(Share))]
public class Share(IShare shareService) : Controller
{
    private readonly IShare _shareService = shareService;

    [HttpPost("{userId}")]
    public async Task<IActionResult> Upload([FromForm] IFormCollection form, [FromRoute] int userId)
    {
        await _shareService.Transfer(form.Files[0], userId);
        return Ok(string.Empty);
    }
}
