namespace ReferenceDatatransfomer;

public class SpendDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string DateTime { get; set; }

        [JsonRequired]
        public string Amount { get; set; }
    }
}
