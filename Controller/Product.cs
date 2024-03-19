namespace ReferenceController;

[ApiController]
[Route(nameof(Product))]
public class Product(IProduct productService) : Controller
{
    private readonly IProduct _productService = productService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var products = _productService.List();
        return Ok(products);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] ProductDatatransfomer.Create create)
    {
        var product = _productService.Create(create);
        return Ok(product);
    }

    [Authorize]
    [HttpDelete("{productId}")]
    public IActionResult Remove([FromRoute] Guid productId)
    {
        var message = _productService.Remove(productId);
        return Ok(message);
    }

    [Authorize]
    [HttpPost(nameof(AddImage) + "/{productId}")]
    public async Task<IActionResult> AddImage([FromForm] IFormCollection form, [FromRoute] Guid productId)
    {
        var imageProduct = await _productService.AddPicture(productId, form.Files);
        return Ok(imageProduct);
    }
}
