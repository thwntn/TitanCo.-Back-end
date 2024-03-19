namespace ReferenceModel;

public class ClientHub(string accountId, string connectionId, IClientProxy clientProxy)
{
    public IClientProxy clientProxy = clientProxy;
    public string connectionId = connectionId;
    public string accountId = accountId;
}
