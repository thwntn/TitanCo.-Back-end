namespace ReferenceController;

[ApiController]
[Route(nameof(Product))]
public class Product(IProduct productService, IJwt jwtService) : Controller
{
    private readonly IProduct _productService = productService;
    private readonly IJwt _jwtService = jwtService;

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var products = _productService.List(_jwtService.ReadToken(Request).userId);
        return Ok(products);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] ProductDatatransfomer.Create create)
    {
        var product = _productService.Create(_jwtService.ReadToken(Request).userId, create);
        return Ok(product);
    }

    [Authorize]
    [HttpPost(nameof(AddImage) + "/{productId}")]
    public async Task<IActionResult> AddImage([FromForm] IFormCollection form, [FromRoute] Guid productId)
    {
        var imageProduct = await _productService.AddPicture(productId, form.Files);
        return Ok(imageProduct);
    }
}
