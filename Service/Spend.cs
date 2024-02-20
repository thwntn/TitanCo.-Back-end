namespace ReferenceService;

public class SpendService(DatabaseContext databaseContext) : ISpend
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Spend> List(int userId, string dateTime)
    {
        var spends = _databaseContext
            .Spend
            .Include(spend => spend.User)
            .Where(spend => spend.UserId == userId && spend.DateTime == dateTime)
            .ToList();
        return spends;
    }

    public Spend Create(int userId, SpendDataTransformer.Create create)
    {
        var spend = NewtonsoftJson.Map<Spend>(create);
        spend.UserId = userId;
        spend.Created = DateTime.Now;
        _databaseContext.Add(spend);
        _databaseContext.SaveChanges();
        return spend;
    }

    public string Remove(int userId, int spendId)
    {
        var spend =
            _databaseContext.Spend.FirstOrDefault(spend => spend.Id == spendId && spend.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_SPEND);
        spend.UserId = userId;
        _databaseContext.Remove(spend);
        _databaseContext.SaveChanges();
        return string.Empty;
    }
}
