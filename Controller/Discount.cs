namespace ReferenceController;

[ApiController]
[Route(nameof(Discount))]
public class Discount(IDiscount discountService, IJwt jwtService) : Controller
{
    private readonly IDiscount _discountService = discountService;
    private readonly IJwt _jwtService = jwtService;

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
    [HttpDelete("{discountId}")]
    public IActionResult Remove([FromRoute] Guid discountId)
    {
        var message = _discountService.Remove(discountId);
        return Ok(message);
    }
}
