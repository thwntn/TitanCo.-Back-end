namespace ReferenceService;

public class SpendService(DatabaseContext databaseContext) : ISpend
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Spend> List(string profileId, string dateTime)
    {
        var spends = _databaseContext
            .Spend.Include(spend => spend.Profile)
            .Where(spend => spend.ProfileId == profileId && spend.DateTime == dateTime)
            .ToList();
        return spends;
    }

    public Spend Create(string profileId, SpendDataTransformer.Create create)
    {
        var spend = NewtonsoftJson.Map<Spend>(create);
        spend.ProfileId = profileId;
        spend.Created = DateTime.Now;
        _databaseContext.Add(spend);
        _databaseContext.SaveChanges();
        return spend;
    }

    public string Remove(string profileId, string spendId)
    {
        var spend =
            _databaseContext.Spend.FirstOrDefault(spend => spend.Id == spendId && spend.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_SPEND);
        spend.ProfileId = profileId;
        _databaseContext.Remove(spend);
        _databaseContext.SaveChanges();
        return string.Empty;
    }
}
