namespace ReferenceDatabase;

public class Note
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    public User User { get; set; }
}
