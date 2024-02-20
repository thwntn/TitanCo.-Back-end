using Microsoft.EntityFrameworkCore.Internal;

namespace ReferenceService;

public class NotificationService(DatabaseContext databaseContext, IWSConnection wsConnectionService) : INotification
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _wsConnectionService = wsConnectionService;

    public List<Notification> List(int userId)
    {
        var notifications = _databaseContext
            .Notification
            .Include(notification => notification.User)
            .Where(notification => notification.UserId == userId)
            .OrderBy(item => item.IsRead)
            .ToList();

        var result = notifications
            .Join(
                _databaseContext.User,
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

    public Notification Add(int userId, int fromUser, NotificationType type, object jsonData)
    {
        Notification notification =
            new()
            {
                JsonData = NewtonsoftJson.Serialize(jsonData),
                Type = type,
                FromId = fromUser,
                IsRead = false,
                Handle = false,
                UserId = userId
            };

        _databaseContext.Add(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(userId);
        return notification;
    }

    public Notification Read(int userId, int notificationId)
    {
        var notification =
            _databaseContext
                .Notification
                .FirstOrDefault(notification => notification.Id == notificationId && notification.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTIFICATION);

        notification.IsRead = true;
        _databaseContext.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(userId);
        return notification;
    }

    public Notification Handle(int userId, int notificationId)
    {
        var notification =
            _databaseContext
                .Notification
                .FirstOrDefault(notification => notification.Id == notificationId && notification.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTIFICATION);

        notification.Handle = true;
        notification.IsRead = true;

        _databaseContext.Notification.Update(notification);
        _databaseContext.SaveChanges();

        RealtimeUpdate(userId);
        return notification;
    }

    private void RealtimeUpdate(int userId)
    {
        _wsConnectionService.InvokeWithUserId(
            string.Concat(userId),
            nameof(HubMethodName.UpdateNotification),
            string.Empty
        );
    }
}
