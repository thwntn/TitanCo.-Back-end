namespace ReferenceController;

[ApiController]
[Route(nameof(Planning))]
public class Planning(IPlanning planningService, IJwt jwtService) : Controller
{
    private readonly IPlanning _planningService = planningService;
    private readonly IJwt _jwtService = jwtService;

    [HttpGet("{weekOfYear}")]
    public IActionResult List([FromRoute] string weekOfYear)
    {
        var plannings = _planningService.List(_jwtService.ReadToken(Request).userId, weekOfYear);
        return Ok(plannings);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PlanningDataTransformer.Create create)
    {
        var planning = _planningService.Create(_jwtService.ReadToken(Request).userId, create);
        return Ok(planning);
    }

    [HttpDelete("{planningId}")]
    public IActionResult Remove([FromRoute] string planningId)
    {
        var message = _planningService.Remove(_jwtService.ReadToken(Request).userId, planningId);
        return Ok(message);
    }
}
