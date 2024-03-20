namespace ReferenceService;

public class ProductService(DatabaseContext databaseContext, IJwt jwtService, IRole roleService) : IProduct
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IRole _roleService = roleService;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Product> List()
    {
        IQueryable<Product> products = _databaseContext
            .Product.Include(product => product.ImageProducts)
            .Include(product => product.InvoiceProducts)
            .Include(product => product.Profile);

        Account account = _jwtService.Account();
        if (
            // assign.full.customer
            _roleService.CheckRoles(account.RoleAccounts, [nameof(RoleContant.ProductShowFull)])
            // onwer.account
            || account.ParentAccountId == Guid.Empty
        )
            return products.Where(product =>
                _jwtService.AccountSystem().Select(account => account.Profile.Id).Contains(product.ProfileId)
            );
        else
            return products.Where(product => product.ProfileId == account.Profile.Id);
    }

    public Product Info(Guid productId)
    {
        IEnumerable<Account> accounts = _jwtService.AccountSystem();

        Product product =
            _databaseContext
                .Product.Include(product => product.ImageProducts)
                .Include(product => product.Profile)
                .FirstOrDefault(product =>
                    product.Id == productId && accounts.Any(item => item.Profile.Id == product.ProfileId)
                ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_PRODUCT);

        return product;
    }

    public Product Update(Guid productId, ProductDatatransfomer.Update update)
    {
        Infomation infomation = _jwtService.Infomation();
        Product product =
            _databaseContext.Product.FirstOrDefault(product =>
                product.Id == productId && product.ProfileId == infomation.profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_PRODUCT);

        product.Description = update.Description;
        product.Name = update.Name;
        product.Price = update.Price;
        product.Sale = update.Sale;

        _databaseContext.Update(product);
        _databaseContext.SaveChanges();
        return product;
    }

    public Product Create(ProductDatatransfomer.Create create)
    {
        Product product = NewtonsoftJson.Map<Product>(create);
        product.Created = DateTime.Now;
        product.ProfileId = _jwtService.Infomation().profileId;

        _databaseContext.Add(product);
        _databaseContext.SaveChanges();
        return product;
    }

    public string Remove(Guid productId)
    {
        Product product =
            _databaseContext
                .Product.Where(product =>
                    product.Id == productId && product.ProfileId == _jwtService.Infomation().profileId
                )
                .FirstOrDefault() ?? throw new HttpException(400, MessageContants.NOT_FOUND_PRODUCT);

        _databaseContext.Remove(product);
        _databaseContext.SaveChanges();

        return string.Empty;
    }

    public async Task<IEnumerable<ImageProduct>> AddPicture(Guid productId, IFormFileCollection files)
    {
        IEnumerable<Task<MStream.Blob>> blob = files.Select(async item => await Reader.Save(item, string.Empty));

        IEnumerable<ImageProduct> imageProducts = (await Task.WhenAll(blob)).Select(item => new ImageProduct
        {
            Url = Reader.CreateURL(item.GetFileName()),
            ProductId = productId
        });

        _databaseContext.AddRange(imageProducts);
        _databaseContext.SaveChanges();

        return imageProducts;
    }
}
