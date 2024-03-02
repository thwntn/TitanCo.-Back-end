namespace ReferenceController;

[ApiController]
[Route(nameof(Spend))]
public class Spend(ISpend spendService, IJwt jwtService) : Controller
{
    private readonly ISpend _spendService = spendService;
    private readonly IJwt _jwtService = jwtService;

    [HttpGet("{dateTime}")]
    public IActionResult List([FromRoute] string dateTime)
    {
        var spends = _spendService.List(_jwtService.ReadToken(Request).userId, dateTime);
        return Ok(spends);
    }

    [HttpPost]
    public IActionResult Create([FromBody] SpendDataTransformer.Create create)
    {
        var spend = _spendService.Create(_jwtService.ReadToken(Request).userId, create);
        return Ok(spend);
    }

    [HttpDelete("{spendId}")]
    public IActionResult Remove([FromRoute] string spendId)
    {
        var message = _spendService.Remove(_jwtService.ReadToken(Request).userId, spendId);
        return Ok(message);
    }
}
