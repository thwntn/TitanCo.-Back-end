namespace ReferenceService;

public class SpendService(DatabaseContext databaseContext, IJwt jwtService) : ISpend
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Spend> List(string dateTime)
    {
        IQueryable<Spend> spends = _databaseContext
            .Spend.Include(spend => spend.Profile)
            .Where(spend => spend.ProfileId == _jwtService.Infomation().profileId && spend.DateTime == dateTime);

        return spends;
    }

    public Spend Create(SpendDataTransformer.Create create)
    {
        Spend spend = NewtonsoftJson.Map<Spend>(create);
        spend.ProfileId = _jwtService.Infomation().profileId;
        spend.Created = DateTime.Now;

        _databaseContext.Add(spend);
        _databaseContext.SaveChanges();

        return spend;
    }

    public string Remove(Guid spendId)
    {
        Spend spend =
            _databaseContext.Spend.FirstOrDefault(spend =>
                spend.Id == spendId && spend.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_SPEND);
        spend.ProfileId = _jwtService.Infomation().profileId;

        _databaseContext.Remove(spend);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
