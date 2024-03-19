namespace ReferenceService;

public class PlanningService(
    DatabaseContext databaseContext,
    INotification notificationService,
    IMail mailService,
    IJwt jwtService
) : IPlanning
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly INotification _notificationService = notificationService;
    private readonly IJwt _jwtService = jwtService;
    private readonly IMail _mailService = mailService;

    public IEnumerable<Planning> List(string weekOfYear)
    {
        IEnumerable<Planning> plannings = _databaseContext.Planning.Where(planning =>
            planning.ProfileId == _jwtService.Infomation().profileId && planning.WeekOfYear == weekOfYear
        );

        return plannings;
    }

    public Planning Create(PlanningDataTransformer.Create create)
    {
        Planning planning = NewtonsoftJson.Map<Planning>(create);
        planning.Created = DateTime.Now;
        planning.ProfileId = _jwtService.Infomation().profileId;

        _databaseContext.Add(planning);
        _databaseContext.SaveChanges();
        return planning;
    }

    public string Remove(Guid planningId)
    {
        Planning planning =
            _databaseContext.Planning.FirstOrDefault(planning =>
                planning.Id == planningId && planning.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_PLANNING);

        _databaseContext.Remove(planning);
        _databaseContext.SaveChanges();
        return string.Empty;
    }

    public void SendNotiOrMail()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm");
        IEnumerable<Planning> plannings = _databaseContext
            .Planning.Include(planning => planning.Profile)
            .Where(planning =>
                (planning.SetEmail == true || planning.SetNotification == true) && planning.DateTime == date
            )
            .ToList();

        IEnumerable<Planning> update = plannings.Where(planning =>
        {
            if (planning.SetNotification && DateTime.Parse(planning.From) <= DateTime.Parse(time))
            {
                _notificationService.Add(planning.ProfileId, planning.ProfileId, NotificationType.ExpirePlan, planning);
                planning.SetNotification = false;
                return true;
            }
            if (planning.SetEmail && DateTime.Parse(planning.From) <= DateTime.Parse(time))
            {
                planning.SetEmail = false;
                _mailService.SendExpirePlanning(planning.Profile.Email, NewtonsoftJson.Serialize(planning));
                return true;
            }
            return false;
        });

        _databaseContext.UpdateRange(update);
        _databaseContext.SaveChanges();
        _databaseContext.ChangeTracker.Clear();
    }
}
