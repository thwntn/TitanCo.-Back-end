namespace ReferenceController;

[ApiController]
[Route(nameof(Profile))]
public class Profile(IProfile profileService) : Controller
{
    private readonly IProfile _profileService = profileService;

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        var info = _profileService.Info();
        return Ok(info);
    }

    [Authorize]
    [HttpPatch]
    public IActionResult Update([FromBody] ProfileDataTransfromer.Update update)
    {
        var info = _profileService.Update(update);
        return Ok(info);
    }

    [Authorize]
    [HttpPost(nameof(ChangeAvatar))]
    public async Task<IActionResult> ChangeAvatar([FromForm] IFormCollection form)
    {
        var user = await _profileService.ChangeAvatar(form.Files[0]);
        return Ok(user);
    }

    [Authorize]
    [HttpPost(nameof(ChangeCoverPicture))]
    public async Task<IActionResult> ChangeCoverPicture([FromForm] IFormCollection form)
    {
        var user = await _profileService.ChangeCoverPicture(form.Files[0]);
        return Ok(user);
    }
}
