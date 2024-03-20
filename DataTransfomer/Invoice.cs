namespace ReferenceDatatransfomer;

public class InvoiceDatatransfomer
{
    public class Create
    {
        [JsonRequired]
        public string Code { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        public Guid DiscountId { get; set; }

        [JsonRequired]
        public List<ProductCreate> InvoiceProducts { get; set; }

        [JsonRequired]
        public Guid ProfileId { get; set; }

        public Guid CustomerId { get; set; }

        [JsonRequired]
        public Guid PaymentId { get; set; }
    }

    public class ProductCreate
    {
        [JsonRequired]
        public Guid ProductId { get; set; }

        [JsonRequired]
        public int Quanlity { get; set; }
    }

    public class ChangeDesctiption
    {
        [JsonRequired]
        public string Desctiption { get; set; }
    }

    public class ChangeCustomer
    {
        [JsonRequired]
        public Guid CustomerId { get; set; }
    }

    public class ChangePaymentMethod
    {
        [JsonRequired]
        public Guid PaymentMethod { get; set; }
    }

    public class RemoveProduct { }

    public class AddProduct
    {
        [JsonRequired]
        public Guid ProductId { get; set; }

        [JsonRequired]
        public Guid InvoiceId { get; set; }

        [JsonRequired]
        public int Quanlity { get; set; }
    }
}
