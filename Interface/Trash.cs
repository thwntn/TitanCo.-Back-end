namespace ReferenceService;

public interface ITrash
{
    public List<Stogare> List(string profileId);
    public Stogare Add(string profileId, string stogareId);
    public Stogare Restore(string profileId, string stogareId);
    public string Remove(string profileId, string stogareId);
}
