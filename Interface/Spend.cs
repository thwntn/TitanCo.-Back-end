namespace ReferenceInterface;

public interface ISpend
{
    List<Spend> List(string profileId, string dateTime);
    Spend Create(string profileId, SpendDataTransformer.Create create);
    string Remove(string profileId, string spendId);
}
