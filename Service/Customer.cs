namespace ReferenceService;

public class CustomerService(DatabaseContext databaseContext) : ICustomer
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Customer> List()
    {
        var customer = _databaseContext.Customer.Include(customer => customer.Invoices).ToList();
        customer.ForEach(item => item.Image = Reader.CreateURL(item.Image));

        return customer;
    }

    public Customer Create(CustomerDataTransfomer.Create create)
    {
        Customer customer = new(create.name, create.phone);
        _databaseContext.Add(customer);
        _databaseContext.SaveChanges();
        return customer;
    }

    public async Task<Customer> AddImage(Guid customerId, IFormFile file)
    {
        var customer =
            _databaseContext.Customer.Find(customerId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_CUSTOMER);
        var save = await Reader.Save(file, string.Empty);
        customer.Image = save.GetFileName();
        _databaseContext.Update(customer);
        _databaseContext.SaveChanges();

        customer.Image = Reader.CreateURL(customer.Image);
        return customer;
    }
}
