namespace ReferenceController;

[ApiController]
[Route(nameof(Discount))]
public class Discount(IDiscount discountService) : Controller
{
    private readonly IDiscount _discountService = discountService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var discounts = _discountService.List();
        return Ok(discounts);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] DiscountDataTransformer.Create create)
    {
        var discount = _discountService.Create(create);
        return Ok(discount);
    }

    [Authorize]
    [HttpPatch(nameof(ChangeStatus) + "/{discountId}")]
    public IActionResult ChangeStatus(
        [FromRoute] Guid discountId,
        [FromBody] DiscountDataTransformer.ChangeStatus changeStatus
    )
    {
        var discount = _discountService.ChangeStatus(discountId, changeStatus.Status);
        return Ok(discount);
    }

    [Authorize]
    [HttpDelete("{discountId}")]
    public IActionResult Remove([FromRoute] Guid discountId)
    {
        var message = _discountService.Remove(discountId);
        return Ok(message);
    }
}
