using System.Diagnostics.CodeAnalysis;

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
    public DateTime DueDate { get; set; }

    [Required]
    public double Sale { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public double Discount { get; set; }

    public ICollection<InvoiceProduct> InvoiceProducts { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [AllowNull]
    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public Guid PaymentId { get; set; }

    public Payment Payment { get; set; }
}
