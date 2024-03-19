namespace ReferenceDatabase;

[PrimaryKey(nameof(InvoiceId), nameof(DiscountId))]
public class InvoiceDiscount
{
    [ForeignKey(nameof(InvoiceId))]
    public Guid InvoiceId { get; set; }

    public Invoice Invoice { get; set; }

    [ForeignKey(nameof(DiscountId))]
    public Guid DiscountId { get; set; }

    public Discount Discount { get; set; }

    [Required]
    public double Percent { get; set; }

    [Required]
    public double Price { get; set; }
}
