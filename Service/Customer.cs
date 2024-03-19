namespace ReferenceService;

public class CustomerService(DatabaseContext databaseContext, IJwt jwtService, IRole roleService) : ICustomer
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IRole _roleService = roleService;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Customer> List()
    {
        IQueryable<Customer> customers = _databaseContext.Customer.Include(customer => customer.Invoices);

        Account account = _jwtService.Account();
        if (
            // assign.full.customer
            _roleService.CheckRoles(account.RoleAccounts, [nameof(RoleContant.CustomerShowFull)])
            // onwer.account
            || account.ParentAccountId == Guid.Empty
        )
            return customers.Where(customer =>
                _jwtService.AccountSystem().Select(account => account.Profile.Id).Contains(customer.ProfileId)
            );
        else
            return customers.Where(customer => customer.ProfileId == account.Profile.Id);
    }

    public Customer Create(CustomerDataTransfomer.Create create)
    {
        Customer customer = new(create.Name, create.Phone, DateTime.Now, _jwtService.Infomation().profileId);

        if (create.FullName is not null)
            customer.FullName = create.FullName;

        _databaseContext.Add(customer);
        _databaseContext.SaveChanges();
        return customer;
    }

    public string Remove(Guid customerId)
    {
        Customer customer =
            _databaseContext.Customer.FirstOrDefault(customer => customer.Id == customerId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_CUSTOMER);

        _databaseContext.Remove(customer);
        _databaseContext.SaveChanges();
        return string.Empty;
    }

    public async Task<Customer> AddImage(Guid customerId, IFormFile file)
    {
        Customer customer =
            _databaseContext.Customer.Find(customerId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_CUSTOMER);
        var save = await Reader.Save(file, string.Empty);

        customer.Image = Reader.CreateURL(save.GetFileName());
        _databaseContext.Update(customer);
        _databaseContext.SaveChanges();

        customer.Image = Reader.CreateURL(customer.Image);
        return customer;
    }
}
