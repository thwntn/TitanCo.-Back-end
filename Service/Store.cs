namespace ReferenceService;

public class WSStore
{
    private static readonly Dictionary<string, List<ClientHub>> _connections = [];

    public static Dictionary<string, List<ClientHub>> GetConnections()
    {
        return _connections;
    }
}
