namespace ReferenceController;

[ApiController]
[Route(nameof(Stogare))]
public class Stogare(IStogare stogareService, IJwt jwtService) : Controller
{
    private readonly IStogare _stogareService = stogareService;
    private readonly IJwt _jwtService = jwtService;

    [Authorize]
    [HttpGet("{stogareId}")]
    public IActionResult List([FromRoute] string stogareId)
    {
        var stogares = _stogareService.List(_jwtService.ReadToken(Request).userId, stogareId);
        return Ok(stogares);
    }

    [Authorize]
    [HttpGet(nameof(Folders))]
    public IActionResult Folders()
    {
        var stogares = _stogareService.Folders(_jwtService.ReadToken(Request).userId);
        return Ok(stogares);
    }

    [Authorize]
    [HttpPost("{stogareId}")]
    public IActionResult Create(
        [FromBody] StogareDataTransfomer.CreateFolder createFolder,
        [FromRoute] string stogareId
    )
    {
        var stogare = _stogareService.CreateFolder(_jwtService.ReadToken(Request).userId, createFolder, stogareId);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] StogareDataTransfomer.Stogare stogare)
    {
        var result = _stogareService.Update(
            _jwtService.ReadToken(Request).userId,
            NewtonsoftJson.Map<ReferenceDatabase.Stogare>(stogare)
        );
        return Ok(result);
    }

    [Authorize]
    [HttpPatch(nameof(Rename) + "/{stogareId}")]
    public IActionResult Rename([FromBody] StogareDataTransfomer.Rename rename, [FromRoute] string stogareId)
    {
        var stogare = _stogareService.Rename(_jwtService.ReadToken(Request).userId, stogareId, rename);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPost(nameof(UploadFile) + "/{stogareId}")]
    public async Task<IActionResult> UploadFile([FromForm] IFormCollection form, [FromRoute] string stogareId)
    {
        var stogares = await _stogareService.Upload(_jwtService.ReadToken(Request).userId, form.Files[0], stogareId);
        return Ok(stogares);
    }

    [Authorize]
    [HttpDelete("{stogareId}")]
    public IActionResult Remove([FromRoute] string stogareId)
    {
        var message = _stogareService.Remove(_jwtService.ReadToken(Request).userId, stogareId);
        return Ok(message);
    }

    [Authorize]
    [HttpGet(nameof(Home))]
    public IActionResult Home()
    {
        var home = _stogareService.Home(_jwtService.ReadToken(Request).userId);
        return Ok(home);
    }

    [Authorize]
    [HttpGet(nameof(Recent))]
    public IActionResult Recent()
    {
        var recent = _stogareService.Recent(_jwtService.ReadToken(Request).userId);
        return Ok(recent);
    }

    [Authorize]
    [HttpGet(nameof(Search))]
    public IActionResult Search([FromQuery] string content)
    {
        var search = _stogareService.Search(_jwtService.ReadToken(Request).userId, content);
        return Ok(search);
    }

    [Authorize]
    [HttpPatch(nameof(Move))]
    public IActionResult Move([FromBody] StogareDataTransfomer.Move move)
    {
        var stogare = _stogareService.Move(_jwtService.ReadToken(Request).userId, move);
        return Ok(stogare);
    }

    [Authorize]
    [HttpGet(nameof(ListDestination) + "/{stogareId}")]
    public IActionResult ListDestination([FromRoute] string stogareId)
    {
        var destinations = _stogareService.ListDestination(_jwtService.ReadToken(Request).userId, stogareId);
        return Ok(destinations);
    }

    [Authorize]
    [HttpGet(nameof(Capture) + "/{stogareId}")]
    public IActionResult Capture([FromRoute] string stogareId)
    {
        var redirect = _stogareService.Redirect(stogareId);
        return Ok(redirect);
    }
}
