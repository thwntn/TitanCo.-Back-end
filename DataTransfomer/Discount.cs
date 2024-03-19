namespace ReferenceDatatransfomer;

public class DiscountDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public double Percent { get; set; }

        [JsonRequired]
        public double Price { get; set; }

        [JsonRequired]
        public int Quanlity { get; set; }
    }

    public class ChangeStatus
    {
        [JsonRequired]
        public DiscountStatus Status { get; set; }
    }
}
