namespace ReferenceController;

[ApiController]
[Route(nameof(User))]
public class User(IUser userService, ISecurity securityService, IAuth authService) : Controller
{
    private readonly IUser _userService = userService;
    private readonly ISecurity _securityService = securityService;
    private readonly IAuth _authService = authService;

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        var info = _userService.Info(_securityService.ReadToken(Request).userId);
        return Ok(info);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] UserDataTransfromer.Update update)
    {
        var user = _userService.Update(_securityService.ReadToken(Request).userId, update);
        return Ok(user);
    }

    [Authorize]
    [HttpPost(nameof(ChangeAvatar))]
    public async Task<IActionResult> ChangeAvatar([FromForm] IFormCollection form)
    {
        var user = await _userService.ChangeAvatar(form.Files[0], _securityService.ReadToken(Request).userId);
        return Ok(user);
    }

    [Authorize]
    [HttpPost(nameof(ChangeCoverPicture))]
    public async Task<IActionResult> ChangeCoverPicture([FromForm] IFormCollection form)
    {
        var user = await _userService.ChangeCoverPicture(form.Files[0], _securityService.ReadToken(Request).userId);
        return Ok(user);
    }
}
