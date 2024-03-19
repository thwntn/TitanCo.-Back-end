namespace ReferenceController;

[ApiController]
[Route(nameof(Customer))]
public class Customer(IJwt jwtService, ICustomer customerService) : Controller
{
    private readonly IJwt _jwtService = jwtService;
    private readonly ICustomer _customerService = customerService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var customers = _customerService.List();
        return Ok(customers);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] CustomerDataTransfomer.Create create)
    {
        var customer = _customerService.Create(create);
        return Ok(customer);
    }

    [Authorize]
    [HttpPost(nameof(AddImage) + "/{customerId}")]
    public async Task<IActionResult> AddImage([FromRoute] Guid customerId, [FromForm] IFormCollection form)
    {
        var customer = await _customerService.AddImage(customerId, form.Files[0]);
        return Ok(customer);
    }

    [Authorize]
    [HttpDelete("{customerId}")]
    public IActionResult Remove([FromRoute] Guid customerId)
    {
        string message = _customerService.Remove(customerId);
        return Ok(message);
    }
}
