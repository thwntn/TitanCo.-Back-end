namespace ReferenceDatatransfomer;

public class NoteDatatransformer
{
    public class Create
    {
        [JsonRequired]
        public string description;

        [JsonRequired]
        public string content;

        [JsonRequired]
        public string name;
    }

    public class UpdateContent
    {
        [JsonRequired]
        public string content;
    }

    public class Update
    {
        [JsonRequired]
        public string id;

        [JsonRequired]
        public DateTime created;

        [JsonRequired]
        public string name;

        [JsonRequired]
        public string description;

        [JsonRequired]
        public StatusNote status;

        [JsonRequired]
        public string content;

        [JsonRequired]
        public string profileId;
    }
}
