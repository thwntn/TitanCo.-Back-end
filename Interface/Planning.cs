namespace ReferenceInterface;

public interface IPlanning
{
    IEnumerable<Planning> List(string weekOfYear);
    Planning Create(PlanningDataTransformer.Create create);
    string Remove(Guid planningId);
    void SendNotiOrMail();
}
