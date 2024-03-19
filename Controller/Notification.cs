namespace ReferenceController;

[ApiController]
[Route(nameof(Notification))]
public class Notification(INotification notificationService) : Controller
{
    private readonly INotification _notificationService = notificationService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var notifications = _notificationService.List();
        return Ok(notifications);
    }

    [Authorize]
    [HttpPatch(nameof(Read) + "/{notificationId}")]
    public IActionResult Read([FromRoute] Guid notificationId)
    {
        var read = _notificationService.Read(notificationId);
        return Ok(read);
    }

    [Authorize]
    [HttpPatch(nameof(Handle) + "/{notificationId}")]
    public IActionResult Handle([FromRoute] Guid notificationId)
    {
        var handle = _notificationService.Handle(notificationId);
        return Ok(handle);
    }
}
