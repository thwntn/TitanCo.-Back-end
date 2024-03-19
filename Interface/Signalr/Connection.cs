namespace ReferenceInterface;

public interface IWSConnection
{
    void Init(IClientProxy clientProxy, string connectionId, string accountId);
    void Disconnect(string connectionId);
    void InvokeWithaccountId(string accountId, string hubMethodName, object data);
    void InvokeAllUser(string hubMethodName, object data);
}
