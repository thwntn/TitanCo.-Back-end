namespace ReferenceInterface;

public interface ISpend
{
    IEnumerable<Spend> List(string dateTime);
    Spend Create(SpendDataTransformer.Create create);
    string Remove(Guid spendId);
}
