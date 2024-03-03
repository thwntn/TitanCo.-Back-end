namespace ReferenceService;

public class WSConnection : IWSConnection
{
    public void Init(IClientProxy clientProxy, string connectionId, string userId)
    {
        var connections = Store.GetInstance().GetConnections();
        if (connections.TryGetValue(userId, out List<ClientHub> value))
            value.Add(new(userId, connectionId, clientProxy));
        else
            connections.Add(userId, [new(userId, connectionId, clientProxy)]);
    }

    public void Disconnect(string connectionId)
    {
        var hubs = Store.GetInstance().GetConnections().SelectMany(item => item.Value).ToList();
        var hub = hubs.Where(item => item.connectionId == connectionId);
        if (hub.Count() is not 0)
            hubs.Remove(hub.First());
    }

    public void InvokeWithUserId(string userId, string hubMethodName, object data)
    {
        if (Store.GetInstance().GetConnections().TryGetValue(userId, out var connection))
            connection.ForEach(async item => await item.clientProxy.SendAsync(hubMethodName, data));
    }

    public void InvokeAllUser(string hubMethodName, object data)
    {
        var hubs = Store.GetInstance().GetConnections().SelectMany(item => item.Value).ToList();
        hubs.ForEach(async item => await item.clientProxy.SendAsync(hubMethodName, data));
    }
}
