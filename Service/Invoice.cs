namespace ReferenceService;

public class InvoiceService(DatabaseContext databaseContext, IJwt jwtService, IRole roleService) : IInvoice
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IRole _roleService = roleService;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Invoice> List()
    {
        IQueryable<Invoice> invoices = _databaseContext
            .Invoice.Include(invoice => invoice.InvoiceDiscounts)
            .ThenInclude(invoiceDiscount => invoiceDiscount.Discount)
            .Include(invoice => invoice.InvoiceProducts)
            .Include(invoice => invoice.Customer)
            .Include(invoice => invoice.Profile)
            .Include(invoice => invoice.Payment);

        Account account = _jwtService.Account();
        if (
            // assign.full.customer
            _roleService.CheckRoles(account.RoleAccounts, [nameof(RoleContant.InvoiceShowFull)])
            // onwer.account
            || account.ParentAccountId == Guid.Empty
        )
            return invoices.Where(invoice =>
                _jwtService.AccountSystem().Select(account => account.Profile.Id).Contains(invoice.ProfileId)
            );
        else
            return invoices.Where(invoice => invoice.ProfileId == account.Profile.Id);
    }

    public Invoice Info(Guid invoiceId)
    {
        Invoice invoice =
            _databaseContext
                .Invoice.Include(invoice => invoice.InvoiceDiscounts)
                .ThenInclude(invoiceDiscount => invoiceDiscount.Discount)
                .Include(invoice => invoice.InvoiceProducts)
                .Include(invoice => invoice.Customer)
                .Include(invoice => invoice.Profile)
                .Include(invoice => invoice.Payment)
                .Where(invoice => invoice.ProfileId == _jwtService.Infomation().profileId && invoice.Id == invoiceId)
                .FirstOrDefault() ?? throw new HttpException(400, MessageContants.NOT_FOUND_INVOICE);
        ;
        return invoice;
    }

    public Invoice Create(InvoiceDatatransfomer.Create create)
    {
        IEnumerable<Product> products = _databaseContext
            .Product.AsEnumerable()
            .Where(product => create.InvoiceProducts.Any(item => item.ProductId == product.Id));

        Invoice invoice = NewtonsoftJson.Map<Invoice>(create);
        invoice.Created = DateTime.Now;
        invoice.Updated = DateTime.Now;
        invoice.ProfileId = _jwtService.Infomation().profileId;

        IEnumerable<InvoiceProduct> invoiceProducts = create.InvoiceProducts.Select(product => new InvoiceProduct(
            invoice.Id,
            product.ProductId,
            products.FirstOrDefault(item => item.Id == product.ProductId).Price,
            product.Quanlity
        ));

        invoice.InvoiceProducts = invoiceProducts.ToList();
        if ((create.DiscountId == Guid.Empty) is false)
        {
            Discount discount =
                _databaseContext.Discount.Where(discount => discount.Id == create.DiscountId).FirstOrDefault()
                ?? throw new HttpException(400, MessageContants.NOT_FOUND_DISCOUNT);
            invoice.InvoiceDiscounts =
            [
                new InvoiceDiscount
                {
                    Discount = discount,
                    Price = discount.Price,
                    Percent = discount.Percent
                }
            ];
        }

        _databaseContext.Add(invoice);
        _databaseContext.SaveChanges();
        return invoice;
    }

    public string Remove(Guid invoiceId)
    {
        Invoice invoice =
            _databaseContext
                .Invoice.Where(invoice =>
                    invoice.Id == invoiceId && invoice.ProfileId == _jwtService.Infomation().profileId
                )
                .FirstOrDefault() ?? throw new HttpException(400, MessageContants.NOT_FOUND_INVOICE);

        _databaseContext.Remove(invoice);
        _databaseContext.SaveChanges();

        return string.Empty;
    }

    public InvoiceProduct AddProduct(InvoiceDatatransfomer.AddProduct addProduct)
    {
        Product product =
            _databaseContext.Product.FirstOrDefault(product =>
                product.Id == addProduct.ProductId && product.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_INVOICE);

        InvoiceProduct invoiceProduct =
            new(addProduct.InvoiceId, addProduct.ProductId, product.Price, addProduct.Quanlity);

        _databaseContext.InvoiceProduct.Add(invoiceProduct);
        return invoiceProduct;
    }
}
