namespace ReferenceController;

[ApiController]
[Route(nameof(Notification))]
public class Notification(INotification notificationService, ISecurity securityService) : Controller
{
    private readonly INotification _notificationService = notificationService;
    private readonly ISecurity _securityService = securityService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var notifications = _notificationService.List(_securityService.ReadToken(Request).userId);
        return Ok(notifications);
    }

    [Authorize]
    [HttpPatch(nameof(Read) + "/{notificationId}")]
    public IActionResult Read([FromRoute] int notificationId)
    {
        var read = _notificationService.Read(_securityService.ReadToken(Request).userId, notificationId);
        return Ok(read);
    }

    [Authorize]
    [HttpPatch(nameof(Handle) + "/{notificationId}")]
    public IActionResult Handle([FromRoute] int notificationId)
    {
        var handle = _notificationService.Handle(_securityService.ReadToken(Request).userId, notificationId);
        return Ok(handle);
    }
}
