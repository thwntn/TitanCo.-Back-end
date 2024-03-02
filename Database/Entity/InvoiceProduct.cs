namespace ReferenceDatabase;

[PrimaryKey(nameof(InvoiceId), nameof(ProductId))]
public class InvoiceProduct
{
    [ForeignKey(nameof(InvoiceId))]
    public Guid InvoiceId { get; set; }

    public Invoice Invoice { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    [Required]
    public int Price { get; set; }

    [Required]
    public int Quanlity { get; set; }
}
