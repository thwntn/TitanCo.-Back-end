namespace ReferenceController;

[ApiController]
[Route(nameof(Token))]
public class Token(IJwt jwtService) : Controller
{
    private readonly IJwt _jwtService = jwtService;

    [HttpGet(nameof(Generate))]
    public IActionResult Generate()
    {
        string jwt = _jwtService.GenerateToken("1");
        return Ok(jwt);
    }

    [Authorize]
    [HttpGet(nameof(Read))]
    public IActionResult Read([FromHeader] string authorization, [FromQuery] string jwt)
    {
        ReferenceModel.JwtPayload claimJwt = _jwtService.ReadToken(Request);
        return Ok(claimJwt);
    }
}
