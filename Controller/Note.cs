namespace ReferenceController;

[ApiController]
[Route(nameof(Note))]
public class Note(INote noteService) : Controller
{
    private readonly INote _noteService = noteService;

    [Authorize]
    [HttpGet(nameof(List) + "/{status}")]
    public IActionResult List(int status)
    {
        var notes = _noteService.List(status);
        return Ok(notes);
    }

    [Authorize]
    [HttpGet("{noteId}")]
    public IActionResult Get([FromRoute] Guid noteId)
    {
        var note = _noteService.Get(noteId);
        return Ok(note);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] NoteDatatransformer.Create create)
    {
        var note = _noteService.Create(create);
        return Ok(note);
    }

    [Authorize]
    [HttpPatch(nameof(MoveToTrash) + "/{noteId}")]
    public IActionResult MoveToTrash([FromRoute] Guid noteId)
    {
        var message = _noteService.MoveToTrash(noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Archive) + "/{noteId}")]
    public IActionResult Archive([FromRoute] Guid noteId)
    {
        var message = _noteService.Archive(noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{noteId}")]
    public IActionResult Restore([FromRoute] Guid noteId)
    {
        var message = _noteService.Restore(noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpDelete("{noteId}")]
    public IActionResult Remove([FromRoute] Guid noteId)
    {
        var message = _noteService.Remove(noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(UpdateContent) + "/{noteId}")]
    public IActionResult UpdateContent(
        [FromRoute] Guid noteId,
        [FromBody] NoteDatatransformer.UpdateContent updateContent
    )
    {
        var note = _noteService.UpdateContent(noteId, updateContent);
        return Ok(note);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] NoteDatatransformer.Update update)
    {
        var note = _noteService.Update(update);
        return Ok(note);
    }
}
