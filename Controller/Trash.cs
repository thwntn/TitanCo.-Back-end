namespace ReferenceController;

[ApiController]
[Route(nameof(Trash))]
public class Trash(ITrash trashService, IJwt jwtService) : Controller
{
    private readonly ITrash _trashService = trashService;
    private readonly IJwt _jwtService = jwtService;

    [HttpGet]
    [Authorize]
    public IActionResult List()
    {
        var list = _trashService.List(_jwtService.ReadToken(Request).userId);
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add([FromBody] TrashDatatransfomer.Add add)
    {
        var stogare = _trashService.Add(_jwtService.ReadToken(Request).userId, add.stogareId);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{stogareId}")]
    public IActionResult Restore([FromRoute] string stogareId)
    {
        var stoagre = _trashService.Restore(_jwtService.ReadToken(Request).userId, stogareId);
        return Ok(stoagre);
    }

    [Authorize]
    [HttpDelete("/{stogareId}")]
    public IActionResult Remove([FromRoute] string stogareId)
    {
        var message = _trashService.Remove(_jwtService.ReadToken(Request).userId, stogareId);
        return Ok(message);
    }
}
