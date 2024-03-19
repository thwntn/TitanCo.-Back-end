namespace ReferenceService;

public class TrashService(DatabaseContext databaseContext, IJwt jwtService) : ITrash
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Stogare> List()
    {
        Profile profile =
            _databaseContext
                .Profile.Include(profile => profile.Stogares)
                .FirstOrDefault(profile => profile.Id == _jwtService.Infomation().profileId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        IEnumerable<Stogare> stogares = profile.Stogares.Where(stogare => stogare.Status == StogareStatus.Trash);
        return stogares;
    }

    public Stogare Add(Guid stogareId)
    {
        Stogare stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == stogareId && stogare.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        stogare.Status = StogareStatus.Trash;
        _databaseContext.Stogare.Update(stogare);
        _databaseContext.SaveChanges();

        return stogare;
    }

    public Stogare Restore(Guid stogareId)
    {
        Stogare stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == stogareId && stogare.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        stogare.Status = StogareStatus.Normal;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        return stogare;
    }

    public string Remove(Guid stogareId)
    {
        Stogare stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == stogareId && stogare.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        _databaseContext.Remove(stogare);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
