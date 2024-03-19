namespace ReferenceController;

[ApiController]
[Route(nameof(Spend))]
public class Spend(ISpend spendService) : Controller
{
    private readonly ISpend _spendService = spendService;

    [HttpGet("{dateTime}")]
    public IActionResult List([FromRoute] string dateTime)
    {
        var spends = _spendService.List(dateTime);
        return Ok(spends);
    }

    [HttpPost]
    public IActionResult Create([FromBody] SpendDataTransformer.Create create)
    {
        var spend = _spendService.Create(create);
        return Ok(spend);
    }

    [HttpDelete("{spendId}")]
    public IActionResult Remove([FromRoute] Guid spendId)
    {
        var message = _spendService.Remove(spendId);
        return Ok(message);
    }
}
