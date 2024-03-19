namespace ReferenceService;

public class PaymentService(DatabaseContext databaseContext, IJwt jwtService) : IPayment
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public Payment Create(PaymentDataTransfomer.Create create)
    {
        Infomation infomation = _jwtService.Infomation();
        Payment payment = new(create.Name, DateTime.Now) { ProfileId = infomation.profileId };

        _databaseContext.Add(payment);
        _databaseContext.SaveChanges();
        return payment;
    }

    public async Task<Payment> AddImage(Guid paymentId, IFormFile file)
    {
        IEnumerable<Guid> profileIds = _jwtService.AccountSystem().Select(account => account.Profile.Id);
        Payment payment =
            _databaseContext.Payment.FirstOrDefault(payment =>
                payment.Id == paymentId && profileIds.Contains(payment.ProfileId)
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_PAYMENT);

        MStream.Save save = await Reader.Save(file, string.Empty);
        payment.Image = Reader.CreateURL(save.GetFileName());

        _databaseContext.Update(payment);
        _databaseContext.SaveChanges();

        return payment;
    }

    public IEnumerable<Payment> List()
    {
        IEnumerable<Guid> profileIds = _jwtService.AccountSystem().Select(account => account.Profile.Id);

        IEnumerable<Payment> payments = _databaseContext.Payment.Where(payment =>
            profileIds.Contains(payment.ProfileId)
        );
        return payments;
    }
}
