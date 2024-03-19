namespace ReferenceController;

[ApiController]
[Route(nameof(Trash))]
public class Trash(ITrash trashService) : Controller
{
    private readonly ITrash _trashService = trashService;

    [HttpGet]
    [Authorize]
    public IActionResult List()
    {
        var list = _trashService.List();
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add([FromBody] TrashDatatransfomer.Add add)
    {
        var stogare = _trashService.Add(add.StogareId);
        return Ok(stogare);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{stogareId}")]
    public IActionResult Restore([FromRoute] Guid stogareId)
    {
        var stoagre = _trashService.Restore(stogareId);
        return Ok(stoagre);
    }

    [Authorize]
    [HttpDelete("/{stogareId}")]
    public IActionResult Remove([FromRoute] Guid stogareId)
    {
        var message = _trashService.Remove(stogareId);
        return Ok(message);
    }
}
