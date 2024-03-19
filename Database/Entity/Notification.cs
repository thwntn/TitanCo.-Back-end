namespace ReferenceDatabase;

public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public NotificationType Type { get; set; }

    [Required]
    public string JsonData { get; set; }

    [Required]
    public bool IsRead { get; set; }

    [Required]
    public bool Handle { get; set; }

    [ForeignKey(nameof(FromId))]
    public Guid FromId { get; set; }

    [NotMapped]
    public Profile From { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }
}
