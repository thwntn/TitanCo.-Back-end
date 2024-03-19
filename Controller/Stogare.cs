namespace ReferenceController;

[ApiController]
[Route(nameof(Stogare))]
public class Stogare(IStogare stogareService) : Controller
{
    private readonly IStogare _stogareService = stogareService;

    [Authorize]
    [HttpGet("{stogareId}")]
    public IActionResult List([FromRoute] Guid stogareId)
    {
        var stogares = _stogareService.List(stogareId);
        return Ok(stogares);
    }

    [Authorize]
    [HttpGet(nameof(Folders))]
    public IActionResult Folders()
    {
        var stogares = _stogareService.Folders();
        return Ok(stogares);
    }

    [Authorize]
    [HttpPost("{stogareId}")]
    public IActionResult Create([FromBody] StogareDataTransfomer.CreateFolder createFolder, [FromRoute] Guid stogareId)
    {
        var stogare = _stogareService.CreateFolder(createFolder, stogareId);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] StogareDataTransfomer.Stogare stogare)
    {
        var result = _stogareService.Update(NewtonsoftJson.Map<ReferenceDatabase.Stogare>(stogare));
        return Ok(result);
    }

    [Authorize]
    [HttpPatch(nameof(Rename) + "/{stogareId}")]
    public IActionResult Rename([FromBody] StogareDataTransfomer.Rename rename, [FromRoute] Guid stogareId)
    {
        var stogare = _stogareService.Rename(stogareId, rename);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPost(nameof(UploadFile) + "/{stogareId}")]
    public async Task<IActionResult> UploadFile([FromForm] IFormCollection form, [FromRoute] Guid stogareId)
    {
        var stogares = await _stogareService.Upload(form.Files[0], stogareId);
        return Ok(stogares);
    }

    [Authorize]
    [HttpDelete("{stogareId}")]
    public IActionResult Remove([FromRoute] Guid stogareId)
    {
        var message = _stogareService.Remove(stogareId);
        return Ok(message);
    }

    [Authorize]
    [HttpGet(nameof(Home))]
    public IActionResult Home()
    {
        var home = _stogareService.Home();
        return Ok(home);
    }

    [Authorize]
    [HttpGet(nameof(Recent))]
    public IActionResult Recent()
    {
        var recent = _stogareService.Recent();
        return Ok(recent);
    }

    [Authorize]
    [HttpGet(nameof(Search))]
    public IActionResult Search([FromQuery] string content)
    {
        var search = _stogareService.Search(content);
        return Ok(search);
    }

    [Authorize]
    [HttpPatch(nameof(Move))]
    public IActionResult Move([FromBody] StogareDataTransfomer.Move move)
    {
        var stogare = _stogareService.Move(move);
        return Ok(stogare);
    }

    [Authorize]
    [HttpGet(nameof(ListDestination) + "/{stogareId}")]
    public IActionResult ListDestination([FromRoute] Guid stogareId)
    {
        var destinations = _stogareService.ListDestination(stogareId);
        return Ok(destinations);
    }

    [Authorize]
    [HttpGet(nameof(Capture) + "/{stogareId}")]
    public IActionResult Capture([FromRoute] Guid stogareId)
    {
        var redirect = _stogareService.Redirect(stogareId);
        return Ok(redirect);
    }
}
