namespace ReferenceDatatransfomer;

public class StogareDataTransfomer
{
    public class CreateFolder
    {
        [JsonRequired]
        public string name;

        public string groupId;
    }

    public class Move
    {
        [JsonRequired]
        public string stogareId;

        [JsonRequired]
        public string destinationId;
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

        public string groupId;
    }
}
