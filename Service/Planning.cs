namespace ReferenceService;

public class PlanningService(DatabaseContext databaseContext, INotification notificationService, IMail mailService)
    : IPlanning
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly INotification _notificationService = notificationService;
    private readonly IMail _mailService = mailService;

    public List<Planning> List(string profileId, string weekOfYear)
    {
        var plannings = _databaseContext
            .Planning.Where(planning => planning.ProfileId == profileId && planning.WeekOfYear == weekOfYear)
            .ToList();
        return plannings;
    }

    public Planning Create(string profileId, PlanningDataTransformer.Create create)
    {
        Planning planning = NewtonsoftJson.Map<Planning>(create);
        planning.Id = Cryptography.RandomGuid();
        planning.Created = DateTime.Now;
        planning.ProfileId = profileId;

        _databaseContext.Add(planning);
        _databaseContext.SaveChanges();
        return planning;
    }

    public string Remove(string profileId, string planningId)
    {
        var planning =
            _databaseContext.Planning.FirstOrDefault(planning =>
                planning.Id == planningId && planning.ProfileId == profileId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_PLANNING);

        _databaseContext.Remove(planning);
        _databaseContext.SaveChanges();
        return string.Empty;
    }

    public void SendNotiOrMail()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm");
        var plannings = _databaseContext
            .Planning.Include(planning => planning.Profile)
            .Where(planning =>
                (planning.SetEmail == true || planning.SetNotification == true) && planning.DateTime == date
            )
            .ToList();

        var update = plannings.Where(planning =>
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
