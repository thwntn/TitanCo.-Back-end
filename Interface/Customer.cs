namespace ReferenceInterface;

public interface ICustomer
{
    IEnumerable<Customer> List();
    Customer Create(CustomerDataTransfomer.Create create);
    Task<Customer> AddImage(Guid customerId, IFormFile file);
    string Remove(Guid customerId);
}
