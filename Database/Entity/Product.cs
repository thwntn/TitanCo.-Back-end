namespace ReferenceDatabase;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public int Price { get; set; }

    [Required]
    public double Sale { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }

    public ICollection<ImageProduct> ImageProducts { get; set; }

    public ICollection<InvoiceProduct> InvoiceProducts { get; set; }
}
