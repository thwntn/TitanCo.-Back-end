namespace ReferenceController;

[ApiController]
[Route(nameof(Health))]
public class Health(IProfile userService) : Controller
{
    private readonly IProfile _accountService = userService;

    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(nameof(Ping));
    }

    [HttpGet(nameof(List))]
    public IActionResult List()
    {
        var info = _accountService.List();
        return Ok(info);
    }
}
