namespace ReferenceDatatransfomer;

public class ProductDatatransfomer
{
    public class Create
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public int price;

        [JsonRequired]
        public string description;

        [JsonRequired]
        public double sale;
    }
}
