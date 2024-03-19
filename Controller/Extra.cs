namespace ReferenceController;

[ApiController]
[Route(nameof(Extra))]
public class Extra(IExtra extraService) : Controller
{
    private readonly IExtra _extraService = extraService;

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] ExtraDatatransformer.Create create)
    {
        var account = _extraService.Create(create);
        return Ok(account);
    }

    [HttpPost(nameof(Signin))]
    public IActionResult Signin([FromBody] ExtraDatatransformer.Signin signin)
    {
        var account = _extraService.Signin(signin);
        return Ok(account);
    }

    [HttpGet]
    public IActionResult List()
    {
        var list = _extraService.List();
        return Ok(list);
    }
}
