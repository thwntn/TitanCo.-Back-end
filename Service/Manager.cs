// namespace ReferenceService;

// public class WSManager(IWSConnection connection) : IWSManager
// {
//     private readonly IWSConnection _connection = connection;

//     public void Sign(SignListenDataTransformer.Sign sign)
//     {
//         Logger.Json(sign);
//         MRoom.Room room = WSStore.GetSignListen().Find(item => item.GetUserId().Equals(sign.userId));
//         if (room is not null)
//         {
//             WSStore.GetSignListen().Add(new(sign.userId, sign.friendId));
//             return;
//         }
//         room.GetListens().Add(sign.friendId);
//     }

//     public void Remove(SignListenDataTransformer.Sign sign)
//     {
//         MRoom.Room room = WSStore
//             .GetSignListen()
//             .Find(item => item.GetUserId().Equals(sign.userId) && item.GetListens().Contains(sign.friendId));
//         room?.GetListens().Remove(sign.friendId);
//     }

//     public void Chat(string userId)
//     {
//         _connection.InvokeWithUserId(userId, nameof(HubMethodName.UpdateMessage), null);
//         List<MRoom.Room> rooms = WSStore
//             .GetSignListen()
//             .FindAll(item => item.GetListens().Contains(userId));
//         rooms.ForEach(
//             item => _connection.InvokeWithUserId(item.GetUserId(), nameof(HubMethodName.UpdateMessage), null)
//         );
//     }
// }
