namespace ReferenceDatatransfomer;

public class ProductDatatransfomer
{
    public class Update
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonRequired]
        public DateTime Created { get; set; }

        [JsonRequired]
        public int Price { get; set; }

        [JsonRequired]
        public double Sale { get; set; }
    }

    public class Create
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public int Price { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonRequired]
        public double Sale { get; set; }
    }
}
