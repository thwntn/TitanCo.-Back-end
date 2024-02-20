namespace ReferenceDatatransfomer;

public class StogareDataTransfomer
{
    public class CreateFolder
    {
        [JsonRequired]
        public string name;

        public int groupId;
    }

    public class Move
    {
        public int stogareId;
        public int destinationId;
    }

    public class Rename
    {
        [JsonRequired]
        public string name;
    }

    public class Stogare
    {
        [JsonRequired]
        public int id;

        [JsonRequired]
        public DateTime created;

        [JsonRequired]
        public string displayName;

        [JsonRequired]
        public string mapName;

        [JsonRequired]
        public string parent;

        [JsonRequired]
        public StogareStatus status;

        [JsonRequired]
        public StogareType type;

        [JsonRequired]
        public string thumbnail;

        [JsonRequired]
        public string url;

        [JsonRequired]
        public int userId;

        public int groupId;
    }
}
