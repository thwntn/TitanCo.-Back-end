namespace ReferenceService;

public interface INotification
{
    IEnumerable<Notification> List();
    Notification Add(Guid toAccount, Guid fromAccount, NotificationType type, object jsonData);
    Notification Read(Guid notificationId);
    Notification Handle(Guid notificationId);
}
