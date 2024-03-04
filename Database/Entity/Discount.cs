namespace ReferenceDatabase;

public class Discount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public double Percent { get; set; }

    [Required]
    public double Price { get; set; }
}
