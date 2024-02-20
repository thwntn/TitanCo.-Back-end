namespace ReferenceModel;

public class ClientHub(string userId, string connectionId, IClientProxy clientProxy)
{
    public IClientProxy clientProxy = clientProxy;
    public string connectionId = connectionId;
    public string userId = userId;
}
