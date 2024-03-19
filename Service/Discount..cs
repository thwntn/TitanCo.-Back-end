namespace ReferenceService;

public class DiscountService(DatabaseContext databaseContext, IJwt jwtService) : IDiscount
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Discount> List()
    {
        IEnumerable<Guid> profileIds = _jwtService.AccountSystem().Select(item => item.Profile.Id);

        IEnumerable<Discount> discounts = _databaseContext.Discount.Where(discount =>
            profileIds.Contains(discount.ProfileId)
        );

        return discounts;
    }

    public Discount Create(DiscountDataTransformer.Create create)
    {
        var discount = NewtonsoftJson.Map<Discount>(create);
        discount.ProfileId = _jwtService.Infomation().profileId;
        discount.Created = DateTime.Now;

        _databaseContext.Add(discount);
        _databaseContext.SaveChanges();
        return discount;
    }

    public string Remove(Guid discountId)
    {
        var discount =
            _databaseContext
                .Discount.Include(discount => discount.InvoiceDiscounts)
                .FirstOrDefault(discount =>
                    discount.Id == discountId && discount.ProfileId == _jwtService.Infomation().profileId
                ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_DISCOUNT);

        if (discount.InvoiceDiscounts.Count > 0)
            throw new HttpException(400, MessageContants.CANNOT_REMOVE_DISCOUNT);

        _databaseContext.Remove(discount);
        _databaseContext.SaveChanges();
        return string.Empty;
    }

    public Discount ChangeStatus(Guid discountId, DiscountStatus discountStatus)
    {
        var discount =
            _databaseContext.Discount.FirstOrDefault(discount =>
                discount.Id == discountId && discount.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_DISCOUNT);

        discount.Status = discountStatus;
        _databaseContext.Update(discount);
        _databaseContext.SaveChanges();

        return discount;
    }
}
