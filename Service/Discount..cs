namespace ReferenceService;

public class DiscountService(DatabaseContext databaseContext) : IDiscount
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Discount> List()
    {
        var discounts = _databaseContext.Discount.ToList();
        return discounts;
    }

    public Discount Create(DiscountDataTransformer.Create create)
    {
        var discount = NewtonsoftJson.Map<Discount>(create);
        _databaseContext.Add(discount);
        _databaseContext.SaveChanges();
        return discount;
    }

    public string Remove(Guid discountId)
    {
        var discount =
            _databaseContext.Discount.Find(discountId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_DISCOUNT);

        _databaseContext.Remove(discount);
        _databaseContext.SaveChanges();
        return string.Empty;
    }
}
