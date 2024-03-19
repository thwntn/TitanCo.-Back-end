namespace ReferenceDatatransfomer;

public class StogareDataTransfomer
{
    public class CreateFolder
    {
        [JsonRequired]
        public string Name { get; set; }

        public Guid GroupId { get; set; }
    }

    public class Move
    {
        [JsonRequired]
        public Guid StogareId { get; set; }

        [JsonRequired]
        public Guid DestinationId { get; set; }
    }

    public class Rename
    {
        [JsonRequired]
        public string Name { get; set; }
    }

    public class Stogare
    {
        [JsonRequired]
        public int Id { get; set; }

        [JsonRequired]
        public DateTime Created { get; set; }

        [JsonRequired]
        public string DisplayName { get; set; }

        [JsonRequired]
        public string MapName { get; set; }

        [JsonRequired]
        public string Parent { get; set; }

        [JsonRequired]
        public StogareStatus Status { get; set; }

        [JsonRequired]
        public StogareType Type { get; set; }

        [JsonRequired]
        public string Thumbnail { get; set; }

        [JsonRequired]
        public string Url { get; set; }

        [JsonRequired]
        public int AccountId { get; set; }

        public Guid GroupId { get; set; }
    }
}
