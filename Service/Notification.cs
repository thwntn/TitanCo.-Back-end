namespace ReferenceService;

public class NotificationService(
    DatabaseContext databaseContext,
    IWSConnection wsConnectionService,
    IJwt jwtService
) : INotification
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _wsConnectionService = wsConnectionService;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Notification> List()
    {
        var notifications = _databaseContext
            .Notification.Include(notification => notification.Profile)
            .Where(notification =>
                notification.ProfileId == _jwtService.Infomation().profileId
            )
            .OrderBy(item => item.IsRead)
            .ToList();

        var result = notifications.Join(
            _databaseContext.Profile,
            notification => notification.FromId,
            user => user.Id,
            (notification, user) =>
            {
                notification.From = user;
                return notification;
            }
        );

        return result;
    }

    public Notification Add(
        Guid toAccount,
        Guid fromAccount,
        NotificationType type,
        object jsonData
    )
    {
        Notification notification =
            new()
            {
                JsonData = NewtonsoftJson.Serialize(jsonData),
                Type = type,
                FromId = fromAccount,
                IsRead = false,
                Handle = false,
                ProfileId = _jwtService.Infomation().profileId
            };

        _databaseContext.Add(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(_jwtService.Infomation().profileId);
        return notification;
    }

    public Notification Read(Guid notificationId)
    {
        var notification =
            _databaseContext.Notification.FirstOrDefault(notification =>
                notification.Id == notificationId
                && notification.ProfileId == _jwtService.Infomation().profileId
            )
            ?? throw new HttpException(
                400,
                MessageContants.NOT_FOUND_NOTIFICATION
            );

        notification.IsRead = true;
        _databaseContext.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(_jwtService.Infomation().profileId);
        return notification;
    }

    public Notification Handle(Guid notificationId)
    {
        var notification =
            _databaseContext.Notification.FirstOrDefault(notification =>
                notification.Id == notificationId
                && notification.ProfileId == _jwtService.Infomation().profileId
            )
            ?? throw new HttpException(
                400,
                MessageContants.NOT_FOUND_NOTIFICATION
            );

        notification.Handle = true;
        notification.IsRead = true;

        _databaseContext.Notification.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(_jwtService.Infomation().profileId);
        return notification;
    }

    private void RealtimeUpdate(Guid profileId)
    {
        _wsConnectionService.InvokeWithaccountId(
            string.Concat(profileId),
            nameof(HubMethodName.UpdateNotification),
            string.Empty
        );
    }
}
