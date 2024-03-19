namespace ReferenceInterface;

public interface IDiscount
{
    IEnumerable<Discount> List();
    Discount Create(DiscountDataTransformer.Create create);
    Discount ChangeStatus(Guid discountId, DiscountStatus discountStatus);
    string Remove(Guid discountId);
}
