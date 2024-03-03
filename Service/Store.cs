namespace ReferenceService;

public class Store
{
    private readonly Dictionary<string, List<ClientHub>> _connections = [];
    private static Store _instance;

    private Store() { }

    public static Store GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public Dictionary<string, List<ClientHub>> GetConnections()
    {
        return _connections;
    }
}
