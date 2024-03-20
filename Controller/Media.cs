namespace ReferenceController;

[ApiController]
[Route(nameof(Media))]
public class Media(IAuth authService) : Controller
{
    private readonly IAuth _authService = authService;

    [HttpGet(nameof(File) + "/{fileName}")]
    public IActionResult File([FromRoute] string fileName)
    {
        FileStream file = Reader.ReadFile(fileName);
        return File(file, "application/octet-stream");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormCollection form, string name)
    {
        MStream.Blob blob = await Reader.Save(form.Files[0], string.Empty);
        blob.SetPath(Reader.CreateURL(blob.GetPath()));
        return Ok(blob);
    }
}
