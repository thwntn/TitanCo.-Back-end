namespace ReferenceInterface;

public interface IWSConnection
{
    void Init(IClientProxy clientProxy, string connectionId, string userId);
    void Disconnect(string connectionId);
    void InvokeWithUserId(string userId, string hubMethodName, object data);
    void InvokeAllUser(string hubMethodName, object data);
}
