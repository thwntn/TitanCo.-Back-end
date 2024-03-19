namespace ReferenceModel;

public class MRoom
{
    public class Room(string accountId, string friendId)
    {
        private readonly string _accountId = accountId;
        private readonly List<string> _listens = [friendId];

        public string GetaccountId() => _accountId;

        public List<string> GetListens() => _listens;
    }
}
