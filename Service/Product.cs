namespace ReferenceService;

public class ProductService(DatabaseContext databaseContext) : IProduct
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Product> List(string profileId)
    {
        var products = _databaseContext
            .Product.Include(product => product.ImageProducts)
            .Include(product => product.Profile)
            .Where(product => product.ProfileId == profileId)
            .ToList();
        return products;
    }

    public Product Create(string profileId, ProductDatatransfomer.Create create)
    {
        var product = NewtonsoftJson.Map<Product>(create);
        product.Created = DateTime.Now;
        product.ProfileId = profileId;

        _databaseContext.Add(product);
        _databaseContext.SaveChanges();
        return product;
    }

    public string Remove(string profileId, Guid productId)
    {
        var product =
            _databaseContext
                .Product.Where(product => product.Id == productId && product.ProfileId == profileId)
                .FirstOrDefault() ?? throw new HttpException(400, MessageDefine.NOT_FOUND_PRODUCT);
        _databaseContext.Remove(product);
        return string.Empty;
    }

    public async Task<List<ImageProduct>> AddPicture(Guid productId, IFormFileCollection files)
    {
        var save = files.Select(async item => await Reader.Save(item, string.Empty));

        var imageProducts = (await Task.WhenAll(save))
            .Select(item => new ImageProduct { Url = Reader.CreateStogare(item.GetFileName()), ProductId = productId })
            .ToList();

        _databaseContext.AddRange(imageProducts);
        _databaseContext.SaveChanges();

        return imageProducts;
    }
}
