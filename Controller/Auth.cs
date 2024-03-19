namespace ReferenceController;

[ApiController]
[Route(nameof(Auth))]
public class Auth(DatabaseContext db, IAuth authService, IJwt jwtService, IExtra subAccountServuce) : Controller
{
    private readonly DatabaseContext _db = db;
    private readonly IAuth _authService = authService;
    private readonly IExtra _subAccountServuce = subAccountServuce;
    private readonly IJwt _jwtService = jwtService;
    private readonly string _redirectUri = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GoogleRedirect));

    [HttpPost(nameof(Signup))]
    public async Task<IActionResult> Signup(AuthDataTransformer.Signup signup)
    {
        Thread.Sleep(Constant.DELAY_API);
        var account = await _authService.Signup(signup);
        return Ok(account);
    }

    [HttpPost(nameof(VerifyEmail))]
    public IActionResult VerifyEmail(AuthDataTransformer.VerifyEmail verifyEmail)
    {
        Thread.Sleep(Constant.DELAY_API);
        var message = _authService.VerifyEmail(verifyEmail);
        return Ok(message);
    }

    [HttpPost(nameof(SigninWithPassword))]
    public async Task<IActionResult> SigninWithPassword([FromBody] AuthDataTransformer.Signin signin)
    {
        Thread.Sleep(Constant.DELAY_API);
        var account = await _authService.SigninWithPassword(signin);
        return Ok(account);
    }

    [HttpPost(nameof(ConfirmCode))]
    public IActionResult ConfirmCode(AuthDataTransformer.ConfirmCode confirmCode)
    {
        Thread.Sleep(Constant.DELAY_API);
        var account = _authService.ConfirmCode(confirmCode.AccountId, confirmCode.Code);
        return Ok(account);
    }

    [HttpPatch(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword([FromBody] AuthDataTransformer.ResetPassword resetPassword)
    {
        Thread.Sleep(Constant.DELAY_API);
        var message = await _authService.ResetPassword(resetPassword.Email);
        return Ok(message);
    }

    [HttpPatch(nameof(ChangePassword))]
    public IActionResult ChangePassword([FromBody] AuthDataTransformer.ChangePassword changePassword)
    {
        Thread.Sleep(Constant.DELAY_API);
        var message = _authService.ChangePassword(changePassword);
        return Ok(message);
    }

    [HttpGet(nameof(Google))]
    public async Task<IActionResult> Google([FromQuery] string code)
    {
        Thread.Sleep(Constant.DELAY_API);
        var profile = await _authService.LoginGoogle(code);
        return Ok(profile);
    }
}
