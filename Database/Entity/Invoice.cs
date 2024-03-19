namespace ReferenceDatabase;

public class Invoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime Updated { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public List<InvoiceProduct> InvoiceProducts { get; set; }

    public List<InvoiceDiscount> InvoiceDiscounts { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }

    [AllowNull]
    [ForeignKey(nameof(CustomerId))]
    public Guid CustomerId { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Customer Customer { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public Guid PaymentId { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Payment Payment { get; set; }
}
