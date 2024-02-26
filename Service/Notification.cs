using Microsoft.EntityFrameworkCore.Internal;

namespace ReferenceService;

public class NotificationService(DatabaseContext databaseContext, IWSConnection wsConnectionService) : INotification
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _wsConnectionService = wsConnectionService;

    public List<Notification> List(string profileId)
    {
        var notifications = _databaseContext
            .Notification.Include(notification => notification.Profile)
            .Where(notification => notification.ProfileId == profileId)
            .OrderBy(item => item.IsRead)
            .ToList();

        var result = notifications
            .Join(
                _databaseContext.Profile,
                notification => notification.FromId,
                user => user.Id,
                (notification, user) =>
                {
                    notification.From = user;
                    return notification;
                }
            )
            .ToList();
        return result;
    }

    public Notification Add(string profileId, string fromUser, NotificationType type, object jsonData)
    {
        Notification notification =
            new()
            {
                JsonData = NewtonsoftJson.Serialize(jsonData),
                Type = type,
                FromId = fromUser,
                IsRead = false,
                Handle = false,
                ProfileId = profileId
            };

        _databaseContext.Add(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(profileId);
        return notification;
    }

    public Notification Read(string profileId, string notificationId)
    {
        var notification =
            _databaseContext.Notification.FirstOrDefault(notification =>
                notification.Id == notificationId && notification.ProfileId == profileId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTIFICATION);

        notification.IsRead = true;
        _databaseContext.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(profileId);
        return notification;
    }

    public Notification Handle(string profileId, string notificationId)
    {
        var notification =
            _databaseContext.Notification.FirstOrDefault(notification =>
                notification.Id == notificationId && notification.ProfileId == profileId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTIFICATION);

        notification.Handle = true;
        notification.IsRead = true;

        _databaseContext.Notification.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(profileId);
        return notification;
    }

    private void RealtimeUpdate(string profileId)
    {
        _wsConnectionService.InvokeWithUserId(
            string.Concat(profileId),
            nameof(HubMethodName.UpdateNotification),
            string.Empty
        );
    }
}
