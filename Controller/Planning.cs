namespace ReferenceController;

[ApiController]
[Route(nameof(Planning))]
public class Planning(IPlanning planningService, ISecurity securityService) : Controller
{
    private readonly IPlanning _planningService = planningService;
    private readonly ISecurity _securityService = securityService;

    [HttpGet("{weekOfYear}")]
    public IActionResult List([FromRoute] string weekOfYear)
    {
        var plannings = _planningService.List(_securityService.ReadToken(Request).userId, weekOfYear);
        return Ok(plannings);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PlanningDataTransformer.Create create)
    {
        var planning = _planningService.Create(_securityService.ReadToken(Request).userId, create);
        return Ok(planning);
    }

    [HttpDelete("{planningId}")]
    public IActionResult Remove([FromRoute] int planningId)
    {
        var message = _planningService.Remove(_securityService.ReadToken(Request).userId, planningId);
        return Ok(message);
    }
}
