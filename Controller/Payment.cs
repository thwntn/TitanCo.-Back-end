namespace ReferenceController;

[ApiController]
[Route(nameof(Payment))]
public class Payment(IJwt jwtService, IPayment paymentService) : Controller
{
    private readonly IJwt _jwtService = jwtService;
    private readonly IPayment _paymentService = paymentService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var payments = _paymentService.List();
        return Ok(payments);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] PaymentDataTransfomer.Create create)
    {
        var payment = _paymentService.Create(create);
        return Ok(payment);
    }

    [Authorize]
    [HttpPost(nameof(AddImage) + "/{paymentId}")]
    public async Task<IActionResult> AddImage([FromRoute] Guid paymentId, [FromBody] IFormCollection form)
    {
        var payment = await _paymentService.AddImage(paymentId, form.Files[0]);
        return Ok(payment);
    }
}
