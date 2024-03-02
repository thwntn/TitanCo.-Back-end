namespace ReferenceController;

[ApiController]
[Route(nameof(Invoice))]
public class Invoice(IJwt jwtSevice, IInvoice invoiceService) : Controller
{
    private readonly IJwt _jwtService = jwtSevice;
    private readonly IInvoice _invoiceService = invoiceService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var invoices = _invoiceService.List(_jwtService.ReadToken(Request).userId);
        return Ok(invoices);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] InvoiceDatatransfomer.Create create)
    {
        var invoice = _invoiceService.Create(_jwtService.ReadToken(Request).userId, create);
        return Ok(invoice);
    }
}
