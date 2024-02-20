namespace ReferenceDatatransfomer;

public class SpendDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public string dateTime;

        [JsonRequired]
        public string amount;
    }
}
