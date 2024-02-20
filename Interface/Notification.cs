namespace ReferenceService;

public interface INotification
{
    List<Notification> List(int userId);
    Notification Add(int userId, int fromUser, NotificationType type, object jsonData);
    Notification Read(int userId, int notificationId);
    Notification Handle(int userId, int notificationId);
}
