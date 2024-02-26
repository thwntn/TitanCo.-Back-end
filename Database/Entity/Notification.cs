namespace ReferenceDatabase;

public class Notification
{
    [Key]
    public string Id { get; set; }

    [Required]
    public NotificationType Type { get; set; }

    [Required]
    public string JsonData { get; set; }

    [Required]
    public bool IsRead { get; set; }

    [Required]
    public bool Handle { get; set; }

    [ForeignKey(nameof(FromId))]
    public string FromId { get; set; }

    [NotMapped]
    public Profile From { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }
}
