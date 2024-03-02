namespace ReferenceController;

[ApiController]
[Route(nameof(Note))]
public class Note(IJwt jwtService, INote noteService) : Controller
{
    private readonly IJwt _jwtService = jwtService;
    private readonly INote _noteService = noteService;

    [Authorize]
    [HttpGet(nameof(List) + "/{status}")]
    public IActionResult List(int status)
    {
        var notes = _noteService.List(_jwtService.ReadToken(Request).userId, status);
        return Ok(notes);
    }

    [Authorize]
    [HttpGet("{noteId}")]
    public IActionResult Get([FromRoute] string noteId)
    {
        var note = _noteService.Get(_jwtService.ReadToken(Request).userId, noteId);
        return Ok(note);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] NoteDatatransformer.Create create)
    {
        var note = _noteService.Create(_jwtService.ReadToken(Request).userId, create);
        return Ok(note);
    }

    [Authorize]
    [HttpPatch(nameof(MoveToTrash) + "/{noteId}")]
    public IActionResult MoveToTrash([FromRoute] string noteId)
    {
        var message = _noteService.MoveToTrash(_jwtService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Archive) + "/{noteId}")]
    public IActionResult Archive([FromRoute] string noteId)
    {
        var message = _noteService.Archive(_jwtService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(Restore) + "/{noteId}")]
    public IActionResult Restore([FromRoute] string noteId)
    {
        var message = _noteService.Restore(_jwtService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpDelete("{noteId}")]
    public IActionResult Remove([FromRoute] string noteId)
    {
        var message = _noteService.Remove(_jwtService.ReadToken(Request).userId, noteId);
        return Ok(message);
    }

    [Authorize]
    [HttpPatch(nameof(UpdateContent) + "/{noteId}")]
    public IActionResult UpdateContent(
        [FromRoute] string noteId,
        [FromBody] NoteDatatransformer.UpdateContent updateContent
    )
    {
        var note = _noteService.UpdateContent(_jwtService.ReadToken(Request).userId, noteId, updateContent);
        return Ok(note);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] NoteDatatransformer.Update update)
    {
        var note = _noteService.Update(_jwtService.ReadToken(Request).userId, update);
        return Ok(note);
    }
}
