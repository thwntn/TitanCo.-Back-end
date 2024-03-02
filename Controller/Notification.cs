namespace ReferenceController;

[ApiController]
[Route(nameof(Notification))]
public class Notification(INotification notificationService, IJwt jwtService) : Controller
{
    private readonly INotification _notificationService = notificationService;
    private readonly IJwt _jwtService = jwtService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var notifications = _notificationService.List(_jwtService.ReadToken(Request).userId);
        return Ok(notifications);
    }

    [Authorize]
    [HttpPatch(nameof(Read) + "/{notificationId}")]
    public IActionResult Read([FromRoute] string notificationId)
    {
        var read = _notificationService.Read(_jwtService.ReadToken(Request).userId, notificationId);
        return Ok(read);
    }

    [Authorize]
    [HttpPatch(nameof(Handle) + "/{notificationId}")]
    public IActionResult Handle([FromRoute] string notificationId)
    {
        var handle = _notificationService.Handle(_jwtService.ReadToken(Request).userId, notificationId);
        return Ok(handle);
    }
}
