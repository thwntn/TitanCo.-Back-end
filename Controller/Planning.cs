namespace ReferenceController;

[ApiController]
[Route(nameof(Planning))]
public class Planning(IPlanning planningService) : Controller
{
    private readonly IPlanning _planningService = planningService;

    [HttpGet("{weekOfYear}")]
    public IActionResult List([FromRoute] string weekOfYear)
    {
        var plannings = _planningService.List(weekOfYear);
        return Ok(plannings);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PlanningDataTransformer.Create create)
    {
        var planning = _planningService.Create(create);
        return Ok(planning);
    }

    [HttpDelete("{planningId}")]
    public IActionResult Remove([FromRoute] Guid planningId)
    {
        var message = _planningService.Remove(planningId);
        return Ok(message);
    }
}
