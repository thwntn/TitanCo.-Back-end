namespace ReferenceDatabase;

public class Role(string name, string code, DateTime created)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; } = created;

    [Required]
    public string Name { get; set; } = name;

    [AllowNull]
    public string Image { get; set; }

    [Required]
    public string Code { get; set; } = code;
}
