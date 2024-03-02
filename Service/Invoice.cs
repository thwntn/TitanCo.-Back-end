namespace ReferenceService;

public class InvoiceService(DatabaseContext databaseContext) : IInvoice
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Invoice> List(string profileId)
    {
        var invoices = _databaseContext.Invoice.Where(invoice => invoice.ProfileId == profileId).ToList();
        return invoices;
    }

    public Invoice Create(string profileId, InvoiceDatatransfomer.Create create)
    {
        var invoice = NewtonsoftJson.Map<Invoice>(create);
        invoice.Created = DateTime.Now;
        invoice.Updated = DateTime.Now;
        invoice.ProfileId = profileId;

        _databaseContext.Add(invoice);
        _databaseContext.SaveChanges();
        return invoice;
    }

    public string Remove(string profileId, Guid invoiceId)
    {
        var invoice =
            _databaseContext
                .Invoice.Where(invoice => invoice.Id == invoiceId && invoice.ProfileId == profileId)
                .FirstOrDefault() ?? throw new HttpException(400, MessageDefine.NOT_FOUND_INVOICE);
        _databaseContext.Remove(invoice);
        return string.Empty;
    }

    public InvoiceProduct AddProduct(string profileId, InvoiceDatatransfomer.AddProduct addProduct)
    {
        var product =
            _databaseContext.Product.FirstOrDefault(product =>
                product.Id == addProduct.productId && product.ProfileId == profileId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_INVOICE);

        InvoiceProduct invoiceProduct =
            new()
            {
                ProductId = addProduct.productId,
                InvoiceId = addProduct.invoiceId,
                Price = product.Price,
                Quanlity = addProduct.quanlity
            };

        _databaseContext.InvoiceProduct.Add(invoiceProduct);
        return invoiceProduct;
    }
}
