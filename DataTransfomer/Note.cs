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
        public Guid Id { get; set; }

        [JsonRequired]
        public DateTime Created { get; set; }

        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonRequired]
        public StatusNote Status { get; set; }

        [JsonRequired]
        public string Content { get; set; }

        [JsonRequired]
        public Guid ProfileId { get; set; }
    }
}
