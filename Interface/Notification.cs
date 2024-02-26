namespace ReferenceService;

public interface INotification
{
    List<Notification> List(string profileId);
    Notification Add(string profileId, string fromUser, NotificationType type, object jsonData);
    Notification Read(string profileId, string notificationId);
    Notification Handle(string profileId, string notificationId);
}
