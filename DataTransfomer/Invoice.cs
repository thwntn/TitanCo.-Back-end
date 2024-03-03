namespace ReferenceDatatransfomer;

public class InvoiceDatatransfomer
{
    public class Create
    {
        [JsonRequired]
        public string description;
    }

    public class ChangeDesctiption
    {
        [JsonRequired]
        public string desctiption;
    }

    public class ChangeCustomer
    {
        [JsonRequired]
        public Guid customerId;
    }

    public class ChangePaymentMethod
    {
        [JsonRequired]
        public Guid paymentMethod;
    }

    public class RemoveProduct { }

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
