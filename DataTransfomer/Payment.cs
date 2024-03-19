namespace ReferenceDatatransfomer;

public class PaymentDataTransfomer
{
    public class Create
    {
        [JsonRequired]
        public string Name { get; set; }
    }
}
