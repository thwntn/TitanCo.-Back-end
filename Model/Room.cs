namespace ReferenceModel;

public class MRoom
{
    public class Room(string userId, string friendId)
    {
        private readonly string _userId = userId;
        private readonly List<string> _listens = [friendId];

        public string GetUserId() => _userId;

        public List<string> GetListens() => _listens;
    }
}
