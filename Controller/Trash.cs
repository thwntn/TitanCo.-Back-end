namespace ReferenceController;

[ApiController]
[Route(nameof(Trash))]
public class Trash(ITrash trashService, ISecurity securityService) : Controller
{
    private readonly ITrash _trashService = trashService;
    private readonly ISecurity _securityService = securityService;

    [HttpGet]
    [Authorize]
    public IActionResult List()
    {
        var list = _trashService.List(_securityService.ReadToken(Request).userId);
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add([FromBody] TrashDatatransfomer.Add add)
    {
        var stogare = _trashService.Add(_securityService.ReadToken(Request).userId, add.stogareId);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{stogareId}")]
    public IActionResult Restore([FromRoute] int stogareId)
    {
        var stoagre = _trashService.Restore(_securityService.ReadToken(Request).userId, stogareId);
        return Ok(stoagre);
    }

    [Authorize]
    [HttpDelete("/{stogareId}")]
    public IActionResult Remove([FromRoute] int stogareId)
    {
        var message = _trashService.Remove(_securityService.ReadToken(Request).userId, stogareId);
        return Ok(message);
    }
}
