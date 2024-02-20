namespace ReferenceService;

public interface ITrash
{
    public List<Stogare> List(int userId);
    public Stogare Add(int userId, int stogareId);
    public Stogare Restore(int userId, int stogareId);
    public string Remove(int userId, int stogareId);
}
