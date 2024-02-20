namespace ReferenceInterface;

public interface ISpend
{
    List<Spend> List(int userId, string dateTime);
    Spend Create(int userId, SpendDataTransformer.Create create);
    string Remove(int userId, int spendId);
}
