namespace ReferenceInterface;

public interface ICustomer
{
    List<Customer> List();
    Customer Create(CustomerDataTransfomer.Create create);
    Task<Customer> AddImage(Guid customerId, IFormFile file);
}
