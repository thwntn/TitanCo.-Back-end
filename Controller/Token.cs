namespace ReferenceController;

[ApiController]
[Route(nameof(Token))]
public class Token(ISecurity securityService) : Controller
{
    private readonly ISecurity _securityService = securityService;

    [HttpGet(nameof(Generate))]
    public IActionResult Generate()
    {
        string jwt = _securityService.GenerateToken("1");
        return Ok(jwt);
    }

    [Authorize]
    [HttpGet(nameof(Read))]
    public IActionResult Read([FromHeader] string authorization, [FromQuery] string jwt)
    {
        ReferenceModel.JwtPayload claimJwt = _securityService.ReadToken(Request);
        return Ok(claimJwt);
    }
}
