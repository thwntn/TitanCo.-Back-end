namespace ReferenceDatatransfomer;

public class DiscountDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public double percent;

        [JsonRequired]
        public double price;
    }
}
