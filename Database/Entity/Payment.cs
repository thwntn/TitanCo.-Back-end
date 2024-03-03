namespace ReferenceDatabase;

public class Payment(string name)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = name;

    public string Image { get; set; }
}
