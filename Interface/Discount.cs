namespace ReferenceInterface;

public interface IDiscount
{
    List<Discount> List();
    Discount Create(DiscountDataTransformer.Create create);
    string Remove(Guid discountId);
}
