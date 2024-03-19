namespace ReferenceDatabase;

public class Discount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    [DefaultValue(DiscountStatus.Active)]
    public DiscountStatus Status { get; set; }

    [Required]
    public int Quanlity { get; set; }

    [Required]
    public double Percent { get; set; }

    [Required]
    public double Price { get; set; }

    public Guid ProfileId { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Profile Profile { get; set; }

    public List<InvoiceDiscount> InvoiceDiscounts { get; set; }
}
