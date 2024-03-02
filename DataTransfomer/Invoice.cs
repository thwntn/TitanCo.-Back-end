namespace ReferenceDatatransfomer;

public class InvoiceDatatransfomer
{
    public class Create
    {
        [JsonRequired]
        public string code;

        [JsonRequired]
        public DateTime dueDate;

        [JsonRequired]
        public int methodPayment;

        [JsonRequired]
        public double sale;

        [JsonRequired]
        public string description;

        public string customerId;
    }

    public class AddProduct
    {
        [JsonRequired]
        public Guid productId;

        [JsonRequired]
        public Guid invoiceId;

        [JsonRequired]
        public int quanlity;
    }
}
