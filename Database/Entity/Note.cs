namespace ReferenceDatabase;

public class Note
{
    [Key]
    public string Id { get; set; }

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
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }
}
