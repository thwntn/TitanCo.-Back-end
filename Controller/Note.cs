namespace ReferenceController;

[ApiController]
[Route(nameof(Note))]
public class Note(ISecurity securityService, INote noteService) : Controller
{
    private readonly ISecurity _securityService = securityService;
    private readonly INote _noteService = noteService;

    [Authorize]
    [HttpGet(nameof(List) + "/{status}")]
    public IActionResult List(int status)
    {
        var notes = _noteService.List(_securityService.ReadToken(Request).userId, status);
        return Ok(notes);
    }

    [Authorize]
    [HttpGet("{noteId}")]
    public IActionResult Get([FromRoute] int noteId)
    {
        var note = _noteService.Get(_securityService.ReadToken(Request).userId, noteId);
        return Ok(note);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] NoteDatatransformer.Create create)
    {
        var note = _noteService.Create(_securityService.ReadToken(Request).userId, create);
        return Ok(note);
    }

    [Authorize]
    [HttpPatch(nameof(MoveToTrash) + "/{noteId}")]
    public IActionResult MoveToTrash([FromRoute] int noteId)
    {
        var message = _noteService.MoveToTrash(_securityService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Archive) + "/{noteId}")]
    public IActionResult Archive([FromRoute] int noteId)
    {
        var message = _noteService.Archive(_securityService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{noteId}")]
    public IActionResult Restore([FromRoute] int noteId)
    {
        var message = _noteService.Restore(_securityService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpDelete("{noteId}")]
    public IActionResult Remove([FromRoute] int noteId)
    {
        var message = _noteService.Remove(_securityService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(UpdateContent) + "/{noteId}")]
    public IActionResult UpdateContent(
        [FromRoute] int noteId,
        [FromBody] NoteDatatransformer.UpdateContent updateContent
    )
    {
        var note = _noteService.UpdateContent(_securityService.ReadToken(Request).userId, noteId, updateContent);
        return Ok(note);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] NoteDatatransformer.Update update)
    {
        var note = _noteService.Update(_securityService.ReadToken(Request).userId, update);
        return Ok(note);
    }
}
