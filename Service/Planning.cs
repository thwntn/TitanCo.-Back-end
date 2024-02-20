namespace ReferenceService;

public class PlanningService(DatabaseContext databaseContext, INotification notificationService, IMail mailService)
    : IPlanning
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly INotification _notificationService = notificationService;
    private readonly IMail _mailService = mailService;

    public List<Planning> List(int userId, string weekOfYear)
    {
        var plannings = _databaseContext
            .Planning
            .Where(planning => planning.UserId == userId && planning.WeekOfYear == weekOfYear)
            .ToList();
        return plannings;
    }

    public Planning Create(int userId, PlanningDataTransformer.Create create)
    {
        Planning planning = NewtonsoftJson.Map<Planning>(create);
        planning.Created = DateTime.Now;
        planning.UserId = userId;

        _databaseContext.Add(planning);
        _databaseContext.SaveChanges();
        return planning;
    }

    public string Remove(int userId, int planningId)
    {
        var planning =
            _databaseContext.Planning.FirstOrDefault(planning => planning.Id == planningId && planning.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_PLANNING);

        _databaseContext.Remove(planning);
        _databaseContext.SaveChanges();
        return string.Empty;
    }

    public void SendNotiOrMail()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm");
        var plannings = _databaseContext
            .Planning
            .Include(planning => planning.User)
            .Where(
                planning => (planning.SetEmail == true || planning.SetNotification == true) && planning.DateTime == date
            )
            .ToList();

        var update = plannings.Where(planning =>
        {
            if (planning.SetNotification && DateTime.Parse(planning.From) <= DateTime.Parse(time))
            {
                _notificationService.Add(planning.UserId, planning.UserId, NotificationType.ExpirePlan, planning);
                planning.SetNotification = false;
                return true;
            }
            if (planning.SetEmail && DateTime.Parse(planning.From) <= DateTime.Parse(time))
            {
                planning.SetEmail = false;
                _mailService.SendExpirePlanning(planning.User.Email, NewtonsoftJson.Serialize(planning));
                return true;
            }
            return false;
        });

        _databaseContext.UpdateRange(update);
        _databaseContext.SaveChanges();
        _databaseContext.ChangeTracker.Clear();
    }
}
