namespace ReferenceInterface;

public interface IPlanning
{
    List<Planning> List(int userId, string weekOfYear);
    Planning Create(int userId, PlanningDataTransformer.Create create);
    string Remove(int userId, int planningId);
    void SendNotiOrMail();
}
