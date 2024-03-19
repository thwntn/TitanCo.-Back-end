namespace ReferenceDatabase;

public class Note
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public StatusNote Status { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Content { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }
}
