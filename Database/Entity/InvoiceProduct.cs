namespace ReferenceDatabase;

[PrimaryKey(nameof(InvoiceId), nameof(ProductId))]
public class InvoiceProduct(Guid invoiceId, Guid productId, int price, int quanlity)
{
    [ForeignKey(nameof(InvoiceId))]
    public Guid InvoiceId { get; set; } = invoiceId;

    public Invoice Invoice { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Guid ProductId { get; set; } = productId;

    public Product Product { get; set; }

    [Required]
    public int Price { get; set; } = price;

    [Required]
    public int Quanlity { get; set; } = quanlity;
}
