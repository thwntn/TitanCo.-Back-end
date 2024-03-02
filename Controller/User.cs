namespace ReferenceController;

[ApiController]
[Route(nameof(User))]
public class User(IProfile userService, IJwt jwtService, IAuth authService) : Controller
{
    private readonly IProfile _userService = userService;
    private readonly IJwt _jwtService = jwtService;
    private readonly IAuth _authService = authService;

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        var info = _userService.Info(_jwtService.ReadToken(Request).userId);
        return Ok(info);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] ProfileDataTransfromer.Update update)
    {
        var user = _userService.Update(_jwtService.ReadToken(Request).userId, update);
        return Ok(user);
    }

    [Authorize]
    [HttpPost(nameof(ChangeAvatar))]
    public async Task<IActionResult> ChangeAvatar([FromForm] IFormCollection form)
    {
        var user = await _userService.ChangeAvatar(form.Files[0], _jwtService.ReadToken(Request).userId);
        return Ok(user);
    }

    [Authorize]
    [HttpPost(nameof(ChangeCoverPicture))]
    public async Task<IActionResult> ChangeCoverPicture([FromForm] IFormCollection form)
    {
        var user = await _userService.ChangeCoverPicture(form.Files[0], _jwtService.ReadToken(Request).userId);
        return Ok(user);
    }
}
