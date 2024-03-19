namespace ReferenceFeature;

public class MapHub(IWSConnection connection, IWSManager managerRoomHub) : Hub
{
    private readonly IWSConnection _connection = connection;
    private readonly IWSManager _managerRoom = managerRoomHub;

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connection.Disconnect(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(nameof(Init))]
    public void Init(object init)
    {
        MTransform<ConnectionDataTransformer.Init> transfomer = Transformer.Map<ConnectionDataTransformer.Init>(
            init
        );
        if (transfomer.errors.Count > 0)
        {
            Clients.Clients(Context.ConnectionId).SendAsync(nameof(HubMethodName.Error), transfomer);
            return;
        }
        _connection.Init(Clients.Clients(Context.ConnectionId), Context.ConnectionId, transfomer.data.accountId);
    }

    // [HubMethodName(nameof(SignListen))]
    // public async void SignListen(object signListen)
    // {
    //     MTransform<SignListenDataTransformer.Sign> s = Transformer.Map<SignListenDataTransformer.Sign>(
    //         signListen
    //     );
    //     if (s.errors.Count > 0)
    //     {
    //         await Clients.Clients(Context.ConnectionId).SendAsync(nameof(HubMethodName.Error), s);
    //         return;
    //     }
    //     _managerRoom.Sign(s.data);
    // }

    // [HubMethodName(nameof(RemoveListen))]
    // public async void RemoveListen(object signListen)
    // {
    //     MTransform<SignListenDataTransformer.Sign> sign = Transformer.Map<SignListenDataTransformer.Sign>(
    //         signListen
    //     );
    //     if (sign.errors.Count > 0)
    //     {
    //         await Clients.Clients(Context.ConnectionId).SendAsync(nameof(HubMethodName.Error), sign);
    //         return;
    //     }
    //     _managerRoom.Remove(sign.data);
    //     Logger.Json(WSStore.GetSignListen());
    // }
}
