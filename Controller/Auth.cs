namespace ReferenceController;

[ApiController]
[Route(nameof(Auth))]
public class Auth(DatabaseContext db, IAuth authService, ISecurity securityService) : Controller
{
    private readonly DatabaseContext _db = db;
    private readonly IAuth _authService = authService;
    private readonly ISecurity _securityService = securityService;
    private readonly string _redirectUri = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GoogleRedirect));

    [HttpPost(nameof(Signin))]
    public async Task<IActionResult> Signin(AuthDataTransformer.Signin signin)
    {
        var signinObject = await _authService.Signin(signin);
        _securityService.SetCookie(Response.Cookies, signinObject);
        return Ok(signinObject);
    }

    [HttpPost(nameof(Code))]
    public IActionResult Code(AuthDataTransformer.Code code)
    {
        var message = _authService.Code(code.userId, code.code);
        return Ok(message);
    }

    [HttpGet(nameof(Google))]
    public async Task<IActionResult> Google([FromQuery] string code)
    {
        var login = await _authService.LoginGoogle(code);
        return Ok(login);
    }

    [HttpPost(nameof(Signup))]
    public async Task<IActionResult> Signup(AuthDataTransformer.Signup signup)
    {
        var data = await _authService.Signup(signup);
        return Ok(data);
    }
}
