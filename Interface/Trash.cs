namespace ReferenceService;

public interface ITrash
{
    public IEnumerable<Stogare> List();
    public Stogare Add(Guid stogareId);
    public Stogare Restore(Guid stogareId);
    public string Remove(Guid stogareId);
}
