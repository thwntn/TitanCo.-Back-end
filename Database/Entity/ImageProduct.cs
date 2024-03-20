namespace ReferenceDatabase;

public class ImageProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsMain { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Guid ProductId { get; set; }

    [Required]
    public Product Product { get; set; }
}
