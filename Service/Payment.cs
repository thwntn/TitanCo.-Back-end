namespace ReferenceService;

public class PaymentService(DatabaseContext databaseContext) : IPayment
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public Payment Create(PaymentDataTransfomer.Create create)
    {
        Payment payment = new(create.name);

        _databaseContext.Add(payment);
        _databaseContext.SaveChanges();
        return payment;
    }

    public async Task<Payment> AddImage(Guid paymentId, IFormFile file)
    {
        var payment =
            _databaseContext.Payment.Find(paymentId) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_PAYMENT);

        var save = await Reader.Save(file, string.Empty);
        payment.Image = save.GetFileName();

        _databaseContext.Update(payment);
        _databaseContext.SaveChanges();

        payment.Image = Reader.CreateURL(payment.Image);
        return payment;
    }

    public List<Payment> List()
    {
        var payments = _databaseContext.Payment.ToList();
        payments.ForEach(item => item.Image = Reader.CreateURL(item.Image));

        return payments;
    }
}
