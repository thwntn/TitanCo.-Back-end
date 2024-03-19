namespace ReferenceDatabase;

public class Planning
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public string DateTime { get; set; }

    [Required]
    public string WeekOfYear { get; set; }

    [Required]
    public string Hour { get; set; }

    [Required]
    public string From { get; set; }

    [Required]
    public string To { get; set; }

    [Required]
    public string Day { get; set; }

    [Required]
    public string Color { get; set; }

    [Required]
    public bool SetNotification { get; set; }

    [Required]
    public bool SetEmail { get; set; }

    [Required]
    public string Name { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }
}
