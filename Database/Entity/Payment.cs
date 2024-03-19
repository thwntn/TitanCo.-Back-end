namespace ReferenceDatabase;

public class Payment(string name, DateTime created)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; } = created;

    [Required]
    public string Name { get; set; } = name;

    [Required]
    public Guid ProfileId { get; set; }

    [Required]
    public Profile Profile { get; set; }

    public string Image { get; set; }
}
