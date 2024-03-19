namespace ReferenceController;

[ApiController]
[Route(nameof(Invoice))]
public class Invoice(IInvoice invoiceService) : Controller
{
    private readonly IInvoice _invoiceService = invoiceService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var invoices = _invoiceService.List();
        return Ok(invoices);
    }

    [Authorize]
    [HttpGet("{invoiceId}")]
    public IActionResult List([FromRoute] Guid invoiceId)
    {
        var invoice = _invoiceService.Info(invoiceId);
        return Ok(invoice);
    }

    [Authorize]
    [HttpDelete("{invoiceId}")]
    public IActionResult Remove([FromRoute] Guid invoiceId)
    {
        var message = _invoiceService.Remove(invoiceId);
        return Ok(message);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] InvoiceDatatransfomer.Create create)
    {
        var invoice = _invoiceService.Create(create);
        return Ok(invoice);
    }
}
