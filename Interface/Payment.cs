namespace ReferenceInterface;

public interface IPayment
{
    Payment Create(PaymentDataTransfomer.Create create);
    Task<Payment> AddImage(Guid paymentId, IFormFile file);
    IEnumerable<Payment> List();
}
