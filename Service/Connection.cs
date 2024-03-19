namespace ReferenceService;

public class WSConnection : IWSConnection
{
    public void Init(IClientProxy clientProxy, string connectionId, string accountId)
    {
        var connections = Store.GetInstance().GetConnections();
        if (connections.TryGetValue(accountId, out List<ClientHub> value))
            value.Add(new(accountId, connectionId, clientProxy));
        else
            connections.Add(accountId, [new(accountId, connectionId, clientProxy)]);
    }

    public void Disconnect(string connectionId)
    {
        var hubs = Store.GetInstance().GetConnections().SelectMany(item => item.Value).ToList();
        var hub = hubs.Where(item => item.connectionId == connectionId);
        if (hub.Count() is not 0)
            hubs.Remove(hub.First());
    }

    public void InvokeWithaccountId(string accountId, string hubMethodName, object data)
    {
        if (Store.GetInstance().GetConnections().TryGetValue(accountId, out var connection))
            connection.ForEach(async item => await item.clientProxy.SendAsync(hubMethodName, data));
    }

    public void InvokeAllUser(string hubMethodName, object data)
    {
        var hubs = Store.GetInstance().GetConnections().SelectMany(item => item.Value).ToList();
        hubs.ForEach(async item => await item.clientProxy.SendAsync(hubMethodName, data));
    }
}
