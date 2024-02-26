namespace ReferenceInterface;

public interface IPlanning
{
    List<Planning> List(string profileId, string weekOfYear);
    Planning Create(string profileId, PlanningDataTransformer.Create create);
    string Remove(string profileId, string planningId);
    void SendNotiOrMail();
}
