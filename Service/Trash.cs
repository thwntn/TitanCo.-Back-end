namespace ReferenceService;

public class TrashService(DatabaseContext databaseContext) : ITrash
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Stogare> List(int userId)
    {
        var user =
            _databaseContext.User.Include(user => user.Stogares).FirstOrDefault(user => user.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var stogares = user.Stogares.Where(stogare => stogare.Status == StogareStatus.Trash).ToList();
        return stogares;
    }

    public Stogare Add(int userId, int stogareId)
    {
        var stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare => stogare.Id == stogareId && stogare.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        stogare.Status = StogareStatus.Trash;
        _databaseContext.Stogare.Update(stogare);
        _databaseContext.SaveChanges();

        return stogare;
    }

    public Stogare Restore(int userId, int stogareId)
    {
        var stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare => stogare.Id == stogareId && stogare.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        stogare.Status = StogareStatus.Normal;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        return stogare;
    }

    public string Remove(int userId, int stogareId)
    {
        var stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare => stogare.Id == stogareId && stogare.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        _databaseContext.Remove(stogare);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
